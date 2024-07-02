namespace AkaShi.Core.Logic.Abstractions;

public interface IUserDataGetter
{
    /// <summary>
    /// Returns current userId or 0 if no userId is presented
    /// </summary>
    int CurrentUserId { get; }
    string CurrentUsername { get; }

    /// <summary>
    /// Throws exception if not userId is presented
    /// </summary>
    /// <returns></returns>
    int GetCurrentUserIdStrict();
    string GetCurrentUserNameStrict();
}