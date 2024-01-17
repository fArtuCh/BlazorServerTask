namespace Domain;

public readonly struct Result<T> 
{

    public static  Result<T> Error => new (new Exception()) ;

    public static Result<T> ErrorCreate(string MessageError) => new(new Exception(), MessageError);


    public readonly string Message { get; init; }

    public readonly bool IsSuccess { get; init; }

    public readonly bool IsError => !IsSuccess;

    public readonly T Value { get; init; }

    public readonly Exception? Exception { get; init; }

    public string ExepMessage { get => Exception?.Message ?? ""; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8601 // Possible null reference assignment.
    public Result(T value, bool isSuccess = true)
    {
        IsSuccess = isSuccess;
        Value = value;
        Exception = null;
        Message = string.Empty;
    }

    public Result(Exception e)
    {
        IsSuccess = false;
        Value = default;
        Exception = e;
        Message = string.Empty;
    }

    public Result(T value, string message, bool isSuccess = true)
    {
        IsSuccess = isSuccess;
        Value = value;
        Exception = null;
        Message = message;
    }


    public Result(Exception e, string message)
    {
        IsSuccess = false;
        Value = default;
        Exception = e;
        Message = message;
    }
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public static implicit operator Result<T>(T value) => new(value);






}
