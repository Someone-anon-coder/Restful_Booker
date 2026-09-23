Feature: Authentication
    As an API client
    I want to authenticate against the Restful-Booker API
    So that I can perform actions that require a token

    @smoke @auth
    Scenario: Valid credentials return a token
        Given the API is available
        When I authenticate with valid credentials
        Then the response should contain a non-empty token
    
    @auth @negative
    Scenario: Invalid credentials are rejected
        Given the API is available
        When I authenticate with invalid credentials
        Then the response should contain the reason "Bad credentials"
        And the response should not contain a token
