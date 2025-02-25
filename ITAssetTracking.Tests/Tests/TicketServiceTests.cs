using ITAssetTracking.App.Services;
using ITAssetTracking.Core.Entities;
using ITAssetTracking.Core.Interfaces.Services;
using ITAssetTracking.Tests.MockRepos;
using NUnit.Framework;

namespace ITAssetTracking.Tests.Tests;

[TestFixture]
public class TicketServiceTests
{
    private ITicketService GetService()
    {
        return new TicketService(new MockTicketRepo(), new MockEmployeeRepo(), new MockAssetRepo());
    }

    [Test]
    public void AddsTicket_Success()
    {
        ITicketService service = GetService();
        var ticket = new Ticket
        {
            AssetID = 1, TicketPriorityID = 1, TicketStatusID = 1, TicketTypeID = 1,
            ReportedByEmployeeID = 1, IssueDescription = "Some Issue"
        };
        
        var result = service.AddTicket(ticket);
        
        Assert.That(result.Ok, Is.True);
        Assert.That(ticket.DateReported.Date, Is.EqualTo(DateTime.Today));
    }

    [Test]
    public void AddsTicket_Fail_UnresolvedTickets()
    {
        ITicketService service = GetService();
        var ticket = new Ticket
        {
            AssetID = 2, TicketPriorityID = 1, TicketStatusID = 1, TicketTypeID = 1,
            ReportedByEmployeeID = 1, IssueDescription = "Some Issue"
        };
        
        var result = service.AddTicket(ticket);
        Assert.That(result.Ok, Is.False);
        Assert.That(result.Message, Is.EqualTo("There are unresolved tickets for that asset"));
    }

    [Test]
    public void UpdatesTicket_Success()
    {
        ITicketService service = GetService();
        var ticket = new Ticket
        {
            TicketID = 2, AssetID = 2, TicketPriorityID = 2, TicketStatusID = 2, TicketResolutionID = 1,
            TicketTypeID = 2,
            AssignedToEmployeeID = 3, ReportedByEmployeeID = 2, DateReported = new DateTime(2025, 02, 10),
            DateClosed = DateTime.Today, IssueDescription = "Some unresolved Issue"
        };
        var result = service.UpdateTicket(ticket);
        var updatedTicket = service.GetTicket(ticket.TicketID).Data;
        Assert.That(result.Ok, Is.True);
        Assert.That(updatedTicket.DateClosed, Is.EqualTo(DateTime.Today));
    }

    [Test]
    public void UpdatesTicket_Fail_UnresolvedTickets()
    {
        ITicketService service = GetService();

        var ticket = new Ticket
        {
            TicketID = 2, AssetID = 3, TicketPriorityID = 2, TicketStatusID = 2, TicketResolutionID = null,
            TicketTypeID = 2,
            AssignedToEmployeeID = 3, ReportedByEmployeeID = 2, DateReported = new DateTime(2025, 02, 10),
            DateClosed = null, IssueDescription = "Some unresolved Issue"
        };
        var result = service.UpdateTicket(ticket);
        Assert.That(result.Ok, Is.False);
        Assert.That(result.Message, Is.EqualTo("There are unresolved tickets for that asset"));
    }
}