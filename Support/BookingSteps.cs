using FluentAssertions;
using Reqnroll;
using RestfulBooker.Tests.Clients;
using RestfulBooker.Tests.Models;

namespace RestfulBooker.Tests.Support;

[Binding]
public class BookingSteps(ScenarioState state)
{
    BookingClient bookingClient = new(ApiClientFactory.CreateClient());

    [When("I create a booking for {string} {string} with a total price of {int} and deposit {string}")]
    public async Task WhenICreateABookingForWithATotalPriceOfAndDepositPaid(string firstname, string lastname, int totalprice, string depositpaid)
    {
        BookingDates bookingDates = new BookingDates("2020-01-20", "2020-01-22");
        Booking booking = new Booking(firstname, lastname, totalprice, depositpaid.Equals("paid") ? true:false, bookingDates, "pool");

        var response = await bookingClient.CreateAsync(booking);
        state.LastResponse = response;
        state.LastCreatedBooking = booking;
        state.BookingId = response.Data!.BookingId;
    }

    [Then("the booking is created successfully")]
    public async Task ThenTheBookingIsCreatedSuccessfully()
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
    public async Task ThenTheResponseStatusCodeIs(int statuscode)
    {
        ((int)state.LastResponse!.StatusCode).Should().Be(statuscode);
    }
}