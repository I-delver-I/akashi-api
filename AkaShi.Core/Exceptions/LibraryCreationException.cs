namespace AkaShi.Core.Exceptions;

public class LibraryCreationException : Exception
{
    public LibraryCreationException(string message, Exception innerException = null)
        : base(message, innerException) { }
}