namespace Rafting.Application.Common.Exceptions;

public sealed class RaftingConflictException : Exception
{
    public RaftingConflictException(string message) : base(message) { }
}
