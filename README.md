# Restful Booker — C# BDD API Test Suite

[![API Tests](https://github.com/Someone-anon-coder/Restful_Booker/actions/workflows/api-tests.yml/badge.svg)](https://github.com/Someone-anon-coder/Restful_Booker/actions/workflows/api-tests.yml)
![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)
![Reqnroll](https://img.shields.io/badge/BDD-Reqnroll-blue)

A portfolio-scale BDD API test suite written in C# with Reqnroll, RestSharp, NUnit, and FluentAssertions, exercising the public [Restful-Booker](https://restful-booker.herokuapp.com) demo API. It covers token auth and full booking CRUD, including the negative paths and the API's quirks, and runs on every push, every PR, and once a day in GitHub Actions.

This is a demo/practice project against a shared public API, not a production system — expect the occasional flaky run when Heroku's free instance is slow to wake or the API's dataset resets.

## Architecture

```
Restful_Booker/
├── Features/
│   ├── Auth.feature          # authentication scenarios
│   └── Booking.feature       # booking CRUD + negative scenarios
├── Clients/
│   ├── ApiClientFactory.cs   # builds a RestClient with base URL + default headers
│   ├── AuthClient.cs         # POST /auth
│   └── BookingClient.cs      # /booking and /ping calls
├── Models/
│   ├── AuthModels.cs         # AuthRequest, AuthResponse
│   └── BookingModels.cs      # Booking, BookingDates, CreateBookingResponse
├── Support/
│   ├── ScenarioState.cs      # typed per-scenario state (injected)
│   ├── TestConfig.cs         # base URL + creds, env-var overridable
│   ├── Hooks.cs              # ping check, auth setup, booking cleanup
│   ├── AuthSteps.cs          # step definitions for Auth.feature
│   └── BookingSteps.cs       # step definitions for Booking.feature
├── .github/workflows/api-tests.yml
├── reqnroll.json
└── RestfulBooker.Tests.csproj
```

### How state flows

Reqnroll creates one `ScenarioState` instance per scenario and hands it to every step/hook class constructor that asks for it — the same idea as a request-scoped bean, but for a test scenario instead of an HTTP request. `Hooks`, `AuthSteps`, and `BookingSteps` all take `ScenarioState state` as a constructor parameter, so a token fetched in a `[BeforeScenario("requires_auth")]` hook is visible to every step that runs afterwards, without any shared mutable globals or string-keyed context lookups.

Booking IDs created during a scenario are tracked in `ScenarioState.CreatedBookingIds` and deleted in an `[AfterScenario]` hook, so scenarios clean up after themselves instead of leaving orphaned bookings on the shared public API.

## Scenario coverage

| # | Scenario | Endpoint(s) | Tags |
|---|---|---|---|
| 1 | Valid credentials return a token | `POST /auth` | `@smoke @auth` |
| 2 | Invalid credentials are rejected | `POST /auth` | `@auth @negative` |
| 3 | Create a booking and retrieve it | `POST /booking`, `GET /booking/{id}` | `@smoke @crud` |
| 4 | Create bookings with different details (Scenario Outline) | `POST /booking`, `GET /booking/{id}` | `@crud` |
| 5 | Fully update a booking | `PUT /booking/{id}` | `@crud @requires_auth` |
| 6 | Partially update a booking | `PATCH /booking/{id}` | `@crud @requires_auth` |
| 7 | Delete a booking | `DELETE /booking/{id}`, `GET /booking/{id}` | `@crud @requires_auth` |
| 8 | Get a non-existent booking | `GET /booking/{id}` | `@negative` |
| 9 | Update a booking without a token | `PUT /booking/{id}` | `@negative` |
| 10 | Create a booking with a missing required field | `POST /booking` | `@negative` |

`GET /ping` isn't its own scenario — it backs the `Given the API is available` step used in every `Booking.feature` scenario (via `Background`) and the `[BeforeTestRun]` hook that fails the run fast if the API is unreachable.

## How to run

```bash
# all tests
dotnet test

# just the smoke tests
dotnet test --filter Category=smoke

# just the negative-path tests
dotnet test --filter Category=negative

# scenarios that need an auth token
dotnet test --filter Category=requires_auth
```

Base URL and credentials can be overridden with environment variables (defaults point at the public demo instance):

```bash
export RESTFUL_BOOKER_BASE_URL="https://restful-booker.herokuapp.com"
export RESTFUL_BOOKER_USERNAME="admin"
export RESTFUL_BOOKER_PASSWORD="password123"
```

## Known API quirks

The suite asserts against these deliberately, not by accident:

- **`GET /ping` returns `201`**, not `200`.
- **`POST /auth` with bad credentials returns `200`**, with `{"reason":"Bad credentials"}` in the body — not `401`.
- **`DELETE /booking/{id}` returns `201`**, not `204`.
- **Auth is a cookie, not a bearer token.** `PUT`/`PATCH`/`DELETE` expect a `Cookie: token=<token>` header, not `Authorization: Bearer <token>`.
- **`PUT`/`PATCH`/`DELETE` without a token return `403`.**
- **Creating a booking with a missing required field (`firstname`) returns `500`**, not a clean `400`/`422` — the suite only asserts "not `200`" here since the API's own error handling is inconsistent.

## Python ↔ C# mapping

| Python/Behave-style | This project |
|---|---|
| `pytest` | NUnit |
| `requests` | RestSharp |
| Behave / `pytest-bdd` `.feature` files | Reqnroll `.feature` files (compiled to NUnit tests at build time) |
| `@pytest.fixture` | Constructor-injected `ScenarioState` |
| `pytest.ini` markers / `behave` tags | Reqnroll tags → NUnit `Category` (`dotnet test --filter Category=...`) |
| `assert` / `pytest-check` | FluentAssertions (`.Should().Be(...)`) |
| `dataclass` / Pydantic model | C# `record` |
