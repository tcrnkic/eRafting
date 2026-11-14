namespace Rafting.Application.Common.Exceptions;

public sealed class RaftingNotFoundException : Exception
{
    public RaftingNotFoundException(string message) : base(message) { }
}
