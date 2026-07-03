namespace Shared;

public interface IResult 
{
    bool IsSuccess { get; }

    ErrorDetails Error { get; }
}

public readonly struct Result : IResult
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public ErrorDetails Error { get; }

    private Result(bool isSuccess, ErrorDetails error)
        => (IsSuccess, Error) = (isSuccess, error);

    public static Result Failure(ErrorDetails error) => new(false, error);

    public static Result Success() => new(true, ErrorDetails.None);
}

public readonly struct Result<TValue>: IResult
{
    private readonly TValue? _value;
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public ErrorDetails Error { get; }
    private Result(TValue? value, bool isSuccess, ErrorDetails error)
        => (IsSuccess, Error, _value) = (isSuccess, error, value);

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public static implicit operator Result<TValue>(TValue? value) => Create(value);

    public static implicit operator Result<TValue>(ErrorDetails error) => Failure(error);

    public static Result<TValue> Failure(ErrorDetails error) => new(default, false, error);

    public static Result<TValue> Success(TValue value) => new(value, true, ErrorDetails.None);
    public static Result<TValue> Create(TValue? value) => value is not null ? Success(value) : Failure(ErrorDetails.NullValue);
}
