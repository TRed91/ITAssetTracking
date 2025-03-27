using System.ComponentModel.DataAnnotations;
using ITAssetTracking.Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITAssetTracking.MVC.Models;

public class TicketsIndexModel
{
    public List<Ticket> Tickets { get; set; } = new();

    public TicketsOrder Order { get; set; } = TicketsOrder.ReportDate;
    [Display(Name = "Status")]
    public byte? TicketStatusId { get; set; }
    [Display(Name = "Type")]
    public byte? TicketTypeId { get; set; }
    [Display(Name = "Priority")]
    public byte? TicketPriorityId { get; set; }
    public bool OnlyUnassigned { get; set; } = false;
    public string? Search { get; set; }

    public SelectList OrderList { get; set; } = GetOrderList();
    public SelectList? TicketStatusList { get; set; }
    public SelectList? TicketTypeList { get; set; }
    public SelectList? TicketPriorityList { get; set; }

    private static SelectList GetOrderList()
    {
        var items = new List<SelectListItem>
        {
            new SelectListItem("Asset", "1"),
            new SelectListItem("Status", "2"),
            new SelectListItem("Type", "3"),
            new SelectListItem("Priority", "4"),
            new SelectListItem("Report Date", "5"),
        };
        return new SelectList(items, "Value", "Text");
    }
}

public enum TicketsOrder
{
    Asset = 1,
    Status,
    Type,
    Priority,
    ReportDate
}