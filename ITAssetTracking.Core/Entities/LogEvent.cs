using System.ComponentModel.DataAnnotations;

namespace ITAssetTracking.Core.Entities;

public class LogEvent
{
    [Key]
    public int Id { get; set; }
    public byte EventSourceID { get; set; }
    public string Message { get; set; }
    public string MessageTemplate { get; set; }
    public string Level { get; set; }
    public DateTime TimeStamp { get; set; }
    public string? Exception { get; set; }
    public string? Properties { get; set; }
    
    public EventSource EventSource { get; set; }
}