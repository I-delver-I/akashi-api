namespace AkaShi.Core.Exceptions;

public sealed class InvalidUsernameOrPasswordException : Exception
{
    public InvalidUsernameOrPasswordException() : base("Invalid username or password.") { }
}