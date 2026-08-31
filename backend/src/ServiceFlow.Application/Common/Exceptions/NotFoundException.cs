namespace ServiceFlow.Application.Common.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string message)
        : base(message)
    {
    }

    public NotFoundException(string entityName, Guid id)
        : base($"{entityName} with ID '{id}' was not found.")
    {
    }
}