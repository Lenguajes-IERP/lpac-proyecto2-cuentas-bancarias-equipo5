namespace SalesPro.Domain.Exceptions;

public abstract class SalesProException : Exception
{
    protected SalesProException(string message) : base(message)
    {
    }

    public abstract int StatusCode { get; }
    public abstract string ErrorCode { get; }
}

public sealed class ValidationFailureException : SalesProException
{
    public ValidationFailureException(string message) : base(message)
    {
    }

    public override int StatusCode => 400;
    public override string ErrorCode => "validation_error";
}

public sealed class NotFoundException : SalesProException
{
    public NotFoundException(string message) : base(message)
    {
    }

    public override int StatusCode => 404;
    public override string ErrorCode => "not_found";
}

public sealed class ConflictException : SalesProException
{
    public ConflictException(string message) : base(message)
    {
    }

    public override int StatusCode => 409;
    public override string ErrorCode => "conflict";
}
