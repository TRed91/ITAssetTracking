using System.ComponentModel.DataAnnotations;

namespace ITAssetTracking.Core.Entities;

public class EventSource
{
    [Key]
    public byte EventSourceID { get; set; }
    public string EventSourceName { get; set; }
    
    List<LogEvent> LogEvents { get; set; } = new List<LogEvent>();
}