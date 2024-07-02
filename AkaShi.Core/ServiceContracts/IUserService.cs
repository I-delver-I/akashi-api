using AkaShi.Core.DTO.User;

namespace AkaShi.Core.ServiceContracts;

public interface IUserService
{
    Task<ICollection<UserDTO>> GetUsers();
    Task<UserDTO> GetUserById(int id);
    Task<UserDTO> CreateUser(UserRegisterDTO userDto);
    Task UpdateUser(UserDTO userDto);
    Task DeleteUser(int userId);
}