# 🧪 RestfulBooker.Reqnroll.CSharp

[![API Tests](https://github.com/Someone-anon-coder/RestfulBooker.Reqnroll.CSharp/actions/workflows/test-pipeline.yml/badge.svg)](https://github.com/Someone-anon-coder/RestfulBooker.Reqnroll.CSharp/actions)
![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![Reqnroll](https://img.shields.io/badge/BDD-Reqnroll-blue)
![RestSharp](https://img.shields.io/badge/HTTP-RestSharp-green)

A lightweight, production-ready BDD API automation suite built in **C# / .NET 8** using **Reqnroll**, **RestSharp**, and **NUnit**. Designed to demonstrate language-agnostic test framework architecture against the live [Restful-Booker API](https://restful-booker.herokuapp.com).

## 🏗️ Framework Architecture


```

RestfulBooker.Reqnroll.CSharp/
├── Features/             # Gherkin BDD Feature Files
│   ├── Auth.feature
│   └── Booking.feature
├── StepDefinitions/      # Reqnroll Glue Code
│   ├── AuthSteps.cs
│   └── BookingSteps.cs
├── Clients/              # RestSharp API Client Wrappers
│   ├── AuthClient.cs
│   └── BookingClient.cs
├── Models/               # C# Records (DTOs) for Serialization
│   ├── AuthDto.cs
│   └── BookingDto.cs
├── Hooks/                # Reqnroll Context & Setup Hooks
│   └── TestHooks.cs
├── .github/workflows/    # CI/CD Execution Pipeline
│   └── test-pipeline.yml
└── RestfulBooker.Tests.csproj

```

## 🛠️ Tech Stack & Dependencies

- **Framework:** .NET 8 SDK
- **BDD Engine:** Reqnroll (NUnit integration)
- **HTTP Client:** RestSharp
- **Assertions:** FluentAssertions
- **CI/CD:** GitHub Actions

## 🚀 Key Features

- **Strongly-Typed Models:** Uses C# `record` types for clean payload serialization and deserialization.
- **Dynamic Context Injection:** Leverages Reqnroll's dependency injection / `ScenarioContext` to pass dynamic auth tokens and dynamic IDs between steps.
- **Full CRUD Validation:** Automates token generation (`POST`), booking creation (`POST`), fetching (`GET`), updates (`PUT`), and deletion (`DELETE`).

## ⚙️ How to Run Locally

1. **Clone the repository:**
```bash
git clone https://github.com/Someone-anon-coder/RestfulBooker.Reqnroll.CSharp.git
cd RestfulBooker.Reqnroll.CSharp

```

2. **Restore dependencies & build:**
```bash
dotnet build

```


3. **Execute tests:**
```bash
dotnet test --logger "console;verbosity=detailed"

```



## 📊 Sample Scenario

```gherkin
Feature: Booking Management

  @smoke @crud
  Scenario: Create and fetch a new hotel booking
    Given I have a valid hotel booking payload for "Alex" "Rider"
    When I submit a POST request to create the booking
    Then the API response status should be 200 OK
    And retrieving the booking by ID should match "Alex" "Rider"
```