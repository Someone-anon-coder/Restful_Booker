using FluentAssertions;
using Reqnroll;
using RestfulBooker.Tests.Clients;
using RestfulBooker.Tests.Models;
using RestfulBooker.Tests.Support;

namespace RestfulBooker.Tests.StepDefinitions;

[Binding]
public class BookingSteps(ScenarioState state, BookingClient bookingClient)
{
    [When("I create a booking for {string} {string} with a total price of {int} and deposit {string}")]
    public async Task WhenICreateABookingForWithATotalPriceOfAndDepositPaid(string firstname, string lastname, int totalprice, string depositpaid)
    {
        BookingDates bookingDates = new BookingDates("2020-01-20", "2020-01-22");
        Booking booking = new Booking(firstname, lastname, totalprice, depositpaid == "paid", bookingDates, "pool");

        var response = await bookingClient.CreateAsync(booking);
        state.LastResponse = response;
        state.LastCreatedBooking = booking;
        state.BookingId = response.Data!.BookingId;
        state.CreatedBookingIds.Add(state.BookingId);
    }

    [Then("the booking is created successfully")]
    public void ThenTheBookingIsCreatedSuccessfully()
    {
        ((int)state.LastResponse!.StatusCode).Should().Be(200);
        state.LastResponse.Content.Should().NotBeNull();
    }

    [When("I retrieve a booking with a non-existent id")]
    public async Task WhenIRetrieveABookingWithANon_ExistentId()
    {
        var response = await bookingClient.GetAsync(0);
        state.LastResponse = response;
    }

    [Then("retrieving the booking by id returns the same details")]
    public async Task ThenRetrievingTheBookingByIdReturnsTheSameDetails()
    {
        var response = await bookingClient.GetAsync(state.BookingId);
        response.Data.Should().BeEquivalentTo(state.LastCreatedBooking);
    }

    [Then("the response status code is {int}")]
    public void ThenTheResponseStatusCodeIs(int statuscode)
    {
        ((int)state.LastResponse!.StatusCode).Should().Be(statuscode);
    }

    [Given("I have created a booking")]
    public async Task GivenIHaveCreatedABooking()
    {
        BookingDates bookingDates = new BookingDates("2020-02-21", "2020-03-25");
        Booking booking = new Booking("John", "Doe", 123, true, bookingDates, "pool");

        var response = await bookingClient.CreateAsync(booking);
        state.BookingId = response.Data!.BookingId;
        state.LastCreatedBooking = booking;
        state.CreatedBookingIds.Add(state.BookingId);
    }

    [When("I update the booking's first name to {string} and total price to {int}")]
    public async Task WhenIUpdateTheBookingsFirstNameToAndTotalPriceTo(string firstname, int totalprice)
    {
        Booking newBooking = state.LastCreatedBooking! with { Firstname = firstname, TotalPrice = totalprice };
        var response = await bookingClient.UpdateAsync(state.BookingId, newBooking, state.Token);
        
        state.LastResponse = response;
        state.LastCreatedBooking = newBooking;
    }

    [When("I update the booking's additional needs to {string}")]
    public async Task WhenIUpdateTheBookingsAdditionalNeedsTo(string additionalNeeds)
    {
        var response = await bookingClient.PatchAsync(state.BookingId, new {additionalneeds = additionalNeeds}, state.Token);
        state.LastResponse = response;
    }

    [When("I delete a booking")]
    public async Task WhenIDeleteABooking()
    {
        var response = await bookingClient.DeleteAsync(state.BookingId, state.Token);
        state.LastResponse = response;
    }

    [When("I update the booking's first name to {string} without a token")]
    public async Task WhenIUpdateTheBookingsFirstNameToWithoutAToken(string firstname)
    {
        Booking newBooking = state.LastCreatedBooking! with { Firstname = firstname };
        var response = await bookingClient.UpdateAsync(state.BookingId, newBooking, null);
        
        state.LastResponse = response;
        state.LastCreatedBooking = newBooking;
    }

    [When("I create a booking with a missing first name")]
    public async Task WhenICreateABookingWithAMissingFirstName()
    {
        BookingDates bookingDates = new BookingDates("2025-03-20", "2025-03-25");
        Booking booking = new Booking(null!, "Dane", 132, false, bookingDates, "lunch");

        var response = await bookingClient.CreateAsync(booking);
        state.LastCreatedBooking = booking;
        state.LastResponse = response;
    }

    [Then("the booking update is successful")]
    public async Task ThenTheBookingUpdateIsSuccessful()
    {
        var response = await bookingClient.GetAsync(state.BookingId);
        ((int)response.StatusCode).Should().Be(200);
    }

    [Then("retrieving the booking by id shows the first name {string} and total price {int}")]
    public async Task ThenRetrievingTheBookingByIdShowsTheFirstNameAndTotalPrice(string firstname, int totalprice)
    {
        var response = await bookingClient.GetAsync(state.BookingId);
        response.Data?.Firstname.Should().Be(firstname);
        response.Data?.TotalPrice.Should().Be(totalprice);
    }

    [Then("retrieving the booking by id shows the additional needs {string}")]
    public async Task ThenRetrievingTheBookingByIdShowsTheAdditionalNeeds(string additionalNeeds)
    {
        var response = await bookingClient.GetAsync(state.BookingId);
        response.Data?.AdditionalNeeds.Should().Be(additionalNeeds);
    }

    [Then("the other booking fields are unchanged")]
    public async Task ThenTheOtherBookingFieldsAreUnchanged()
    {
        var response = await bookingClient.GetAsync(state.BookingId);
        
        response.Data?.Firstname.Should().Be(state.LastCreatedBooking!.Firstname);
        response.Data?.Lastname.Should().Be(state.LastCreatedBooking!.Lastname);
        response.Data?.TotalPrice.Should().Be(state.LastCreatedBooking!.TotalPrice);
        response.Data?.DepositPaid.Should().Be(state.LastCreatedBooking!.DepositPaid);
        response.Data?.BookingDates.Checkin.Should().Be(state.LastCreatedBooking!.BookingDates.Checkin);
        response.Data?.BookingDates.Checkout.Should().Be(state.LastCreatedBooking!.BookingDates.Checkout);
    }

    [Then("the booking deletion is successful and the response status code is {int}")]
    public void ThenTheBookingDeletionIsSuccessfulAndTheResponseStatusCodeIs(int statuscode)
    {
        ((int)state.LastResponse!.StatusCode).Should().Be(statuscode);
    }

    [Then("retrieving the booking by id returns a {int}")]
    public async Task ThenRetrievingTheBookingByIdReturnsA(int statuscode)
    {
        var response = await bookingClient.GetAsync(state.BookingId);
        ((int)response.StatusCode).Should().Be(statuscode);
    }

    [Then("the response status code is not {int}")]
    public void ThenTheResponseStatusCodeIsNot(int statuscode)
    {
        ((int)state.LastResponse!.StatusCode).Should().NotBe(statuscode);
    }
}