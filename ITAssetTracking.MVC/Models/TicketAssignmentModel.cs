using ITAssetTracking.Core.Entities;

namespace ITAssetTracking.MVC.Models;

public class TicketAssignmentModel
{
    public int TicketId { get; set; }
    public List<Employee> Employees { get; set; }
}