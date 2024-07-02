using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Domain.RepositoryContracts;
using AkaShi.Core.DTO.User;
using AkaShi.Core.Exceptions;
using AkaShi.Core.Security;
using AkaShi.Core.ServiceContracts;
using AkaShi.Core.Services.Abstract;
using AutoMapper;

namespace AkaShi.Core.Services;

public sealed class UserService : BaseService, IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
        _userRepository = UnitOfWork.UserRepository;
    }

    public async Task<ICollection<UserDTO>> GetUsers()
    {
        var users = await _userRepository.GetAllAsync();

        return Mapper.Map<ICollection<UserDTO>>(users);
    }

    public async Task<UserDTO> GetUserById(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
        {
            throw new NotFoundException(nameof(User), id);
        }

        return Mapper.Map<UserDTO>(user);
    }

    public async Task<UserDTO> CreateUser(UserRegisterDTO userDto)
    {
        var userEntity = Mapper.Map<User>(userDto);
        var salt = SecurityHelper.GetRandomBytes();

        userEntity.PasswordSalt = Convert.ToBase64String(salt);
        userEntity.PasswordHash = SecurityHelper.HashPassword(userDto.Password, salt);

        await _userRepository.AddAsync(userEntity);
        await UnitOfWork.SaveAsync();

        return Mapper.Map<UserDTO>(userEntity);
    }

    public async Task UpdateUser(UserDTO userDto)
    {
        var userEntity = await _userRepository.GetByIdAsync(userDto.Id);
        
        if (userEntity is null)
        {
            throw new NotFoundException(nameof(User), userDto.Id);
        }

        userEntity.Email = userDto.Email;
        userEntity.Username = userDto.Username;

        _userRepository.Update(userEntity);
        await UnitOfWork.SaveAsync();
    }

    public async Task DeleteUser(int userId)
    {
        var userEntity = await _userRepository.GetByIdAsync(userId);
        if (userEntity is null)
        {
            throw new NotFoundException(nameof(User), userId);
        }

        _userRepository.Delete(userEntity);
        await UnitOfWork.SaveAsync();
    }
}