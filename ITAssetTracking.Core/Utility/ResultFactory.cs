namespace ITAssetTracking.Core.Utility;

public class Result
{
    public bool Ok { get; set; }
    public string? Message { get; set; }
    public Exception? Exception { get; set; }

    public Result(bool ok, string? message, Exception? exception)
    {
        Ok = ok;
        Message = message;
        Exception = exception;
    }
}

public class Result<T> : Result
{
    public T Data { get; set; }

    public Result(bool ok, T data, string? message, Exception? exception) 
        : base(ok, message, exception)
    {
        Data = data;
    }
}

public static class ResultFactory
{
    public static Result Success()
    {
        return new Result(true, String.Empty, null);
    }

    public static Result Fail(string message, Exception? exception = null)
    {
        return new Result(false, message, exception);
    }

    public static Result<T> Success<T>(T data)
    {
        return new Result<T>(true, data, string.Empty, null);
    }

    public static Result<T> Fail<T>(string message, Exception? exception = null)
    {
        return new Result<T>(false, default, message, exception);
    }
}