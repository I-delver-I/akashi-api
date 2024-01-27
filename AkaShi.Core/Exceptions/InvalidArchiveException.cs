namespace AkaShi.Core.Exceptions;

public class InvalidArchiveException : Exception
{
    public InvalidArchiveException() : base("Invalid archive structure.") { }
}