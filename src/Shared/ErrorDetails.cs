namespace Shared;

public partial record ErrorDetails(string Message, string ErrorCode)
{
    public static readonly ErrorDetails None = new(string.Empty, string.Empty);
    public static readonly ErrorDetails NullValue = new("The specified result value is null.", "Error.NullValue");
    public static readonly ErrorDetails NotFound = new("The specified entity was not found.", "Error.NotFound");
    public static ErrorDetails Aggregate(IEnumerable<ErrorDetails> errors) => 
        new ErrorDetails(string.Join(Environment.NewLine, errors.Select(e => e.Message)), "Error.AggregateError");
}
