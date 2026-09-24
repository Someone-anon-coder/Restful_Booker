Feature: Booking management
    As an API client
    I want to create, read, update, and delete bookings
    So that I can manage reservations through the Restful-Booker API

    Background:
        Given the API is available
    
    @smoke @crud
    Scenario: Create a booking and retrieve it
        When I create a booking for "Jim" "Brown" with a total price of 111 and deposit "paid"
        Then the booking is created successfully
        And retrieving the booking by id returns the same details
    
    @crud
    Scenario Outline: Create bookings with different details
        When I create a booking for "<firstname>" "<lastname>" with a total price of <totalprice> and deposit "<depositstatus>"
        Then the booking is created successfully
        And retrieving the booking by id returns the same details

        Examples: 
            | firstname | lastname | totalprice | depositstatus |
            | Alice     | Smith    | 150        | paid          |
            | Bob       | Jones    | 200        | unpaid        |
            | Carol     | White    | 99         | paid          |

    @crud @requires_auth
    Scenario: Fully update a booking
        Given I have created a booking
        When I update the booking's first name to "Jane" and total price to 250
        Then the booking update is successful
        And retrieving the booking by id shows the first name "Jane" and total price 250

    @crud @requires_auth
    Scenario: Partially update a booking
        Given I have created a booking
        When I update the booking's additional needs to "Breakfast"
        Then the booking update is successful
        And retrieving the booking by id shows the additional needs "Breakfast"
        And the other booking fields are unchanged

    @crud @requires_auth
    Scenario: Delete a booking
        Given I have created a booking
        When I delete a booking
        Then the booking deletion is successful and the response status code is 201
        And retrieving the booking by id returns a 404
    
    @negative
    Scenario: Get a non-existent booking
        When I retrieve a booking with a non-existent id
        Then the response status code is 404

    @negative
    Scenario: Update a booking without a token
        Given I have created a booking
        When I update the booking's first name to "Jane" without a token
        Then the response status code is 403
    
    @negative
    Scenario: Create a booking with a missing required field
        When I create a booking with a missing first name
        Then the response status code is not 200
