﻿namespace ITAssetTracking.Core.Entities;

public class LogEvents
{
    public int Id { get; set; }
    public byte EventSourceID { get; set; }
    public string Message { get; set; }
    public string MessageTemplate { get; set; }
    public string Level { get; set; }
    public DateTime TimeStamp { get; set; }
    public string Exception { get; set; }
    public string Properties { get; set; }
    
    EventSource EventSource { get; set; }
}