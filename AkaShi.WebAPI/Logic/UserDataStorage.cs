using AkaShi.Core.Exceptions;
using AkaShi.Core.Logic.Abstractions;

namespace AkaShi.WebAPI.Logic;

public class UserDataStorage : IUserDataGetter, IUserDataSetter
{
    public int CurrentUserId { get; private set; }
    public string CurrentUsername { get; private set; }

    public int GetCurrentUserIdStrict()
    {
        if (CurrentUserId == 0)
        {
            throw new InvalidTokenException("No token with userId was passed");
        }

        return CurrentUserId;
    }
    
    public string GetCurrentUserNameStrict()
    {
        if (string.IsNullOrEmpty(CurrentUsername))
        {
            throw new InvalidTokenException("No token with userName was passed");
        }

        return CurrentUsername;
    }

    public void SetUserId(int userId)
    {
        CurrentUserId = userId;
    }
    
    public void SetUserName(string userName)
    {
        CurrentUsername = userName;
    }
}