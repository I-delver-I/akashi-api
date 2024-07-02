using System.Net;
using AkaShi.Core.Exceptions;
using AkaShi.WebAPI.Enums;

namespace AkaShi.WebAPI.Extensions;

public static class ExceptionFilterExtensions
{
    public static (HttpStatusCode statusCode, ErrorCode errorCode) ParseException(this Exception exception)
    {
        return exception switch
        {
            NotFoundException _ => (HttpStatusCode.NotFound, ErrorCode.NotFound),
            InvalidUsernameOrPasswordException _ => (HttpStatusCode.Unauthorized, ErrorCode.InvalidUsernameOrPassword),
            InvalidTokenException _ => (HttpStatusCode.Unauthorized, ErrorCode.InvalidToken),
            ExpiredRefreshTokenException _ => (HttpStatusCode.Unauthorized, ErrorCode.ExpiredRefreshToken),
            FirebaseStorageUploadException _ => (HttpStatusCode.InternalServerError, ErrorCode.FirebaseStorageUpload),
            FirebaseStorageRemoveException _ => (HttpStatusCode.InternalServerError, ErrorCode.FirebaseStorageRemove),
            InvalidArchiveException _ => (HttpStatusCode.BadRequest, ErrorCode.InvalidArchive),
            LibraryCreationException _ => (HttpStatusCode.BadRequest, ErrorCode.LibraryCreation),
            LibraryVersionSupportedFrameworkCreationException _ => 
                (HttpStatusCode.BadRequest, ErrorCode.LibraryVersionSupportedFrameworkCreation),
            _ => (HttpStatusCode.InternalServerError, ErrorCode.General),
        };
    }
}