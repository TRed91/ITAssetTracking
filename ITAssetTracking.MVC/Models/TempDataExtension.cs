using Newtonsoft.Json;

namespace ITAssetTracking.MVC.Models;

public static class TempDataExtension
{
    public static string Serialize(TempDataMsg msg)
    {
        return JsonConvert.SerializeObject(msg);
    }

    public static TempDataMsg? Deserialize(string msg)
    {
        return JsonConvert.DeserializeObject<TempDataMsg>(msg);
    }
}

public class TempDataMsg
{
    public bool Success { get; set; }
    public string Message { get; set; }

    public TempDataMsg(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}