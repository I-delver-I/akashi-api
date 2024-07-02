namespace AkaShi.Core.Exceptions;

public class FirebaseStorageRemoveException : Exception
{
    public FirebaseStorageRemoveException(Exception ex) 
        : base("Error occurred while removing file.", ex) { }
}