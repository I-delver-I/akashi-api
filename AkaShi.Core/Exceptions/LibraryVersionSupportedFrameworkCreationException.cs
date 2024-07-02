namespace AkaShi.Core.Exceptions;

public class LibraryVersionSupportedFrameworkCreationException : Exception
{
    public LibraryVersionSupportedFrameworkCreationException(string message, 
        Exception innerException = null) : base(message, innerException) { }
}