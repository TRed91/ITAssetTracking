using System.ComponentModel.DataAnnotations;

namespace ITAssetTracking.Core.Entities;

public class EventSource
{
    [Key]
    public byte EventSourceID { get; set; }
    public string EventSourceName { get; set; }
    
    List<LogEvents> LogEvents { get; set; } = new List<LogEvents>();
}