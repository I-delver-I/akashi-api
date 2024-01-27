namespace AkaShi.Core.Exceptions;

public class FirebaseStorageUploadException : Exception
{
    public FirebaseStorageUploadException(Exception ex) 
        : base("Error occured while uploading to Firebase Storage.", ex) { }
}