namespace AkaShi.WebAPI.Enums;

public enum ErrorCode
{
    General = 1,
    NotFound,
    InvalidUsernameOrPassword,
    InvalidToken,
    ExpiredRefreshToken,
    FirebaseStorageUpload,
    FirebaseStorageRemove,
    InvalidArchive,
    LibraryCreation,
    LibraryVersionSupportedFrameworkCreation
}