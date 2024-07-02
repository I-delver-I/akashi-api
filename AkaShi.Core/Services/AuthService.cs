using AkaShi.Core.Domain.Entities;
using AkaShi.Core.Domain.RepositoryContracts;
using AkaShi.Core.DTO.Auth;
using AkaShi.Core.DTO.User;
using AkaShi.Core.Exceptions;
using AkaShi.Core.JWT;
using AkaShi.Core.Logic.Abstractions;
using AkaShi.Core.Security;
using AkaShi.Core.ServiceContracts;
using AkaShi.Core.Services.Abstract;
using AutoMapper;

namespace AkaShi.Core.Services;

public class AuthService : BaseService, IAuthService
{
    private readonly JwtFactory _jwtFactory;
    private readonly IUserDataGetter _userDataGetter;
    
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public AuthService(IUnitOfWork unitOfWork, IMapper mapper, JwtFactory jwtFactory, IUserDataGetter userDataGetter) 
        : base(unitOfWork, mapper)
    {
        _jwtFactory = jwtFactory;
        _userDataGetter = userDataGetter;
        _userRepository = unitOfWork.UserRepository;
        _refreshTokenRepository = unitOfWork.RefreshTokenRepository;
    }

    public async Task<AuthUserDTO> Authorize(UserLoginDTO userDto)
    {
        var userEntity = (await _userRepository.GetAllAsync())
            .FirstOrDefault(u => u.Username == userDto.Username);

        if (userEntity == null)
        {
            throw new NotFoundException(nameof(User));
        }

        if (!SecurityHelper.ValidatePassword(userDto.Password, userEntity.PasswordHash, userEntity.PasswordSalt))
        {
            throw new InvalidUsernameOrPasswordException();
        }

        var token = await GenerateAccessToken(userEntity.Id, userEntity.Username, userEntity.Email);
        var user = Mapper.Map<UserDTO>(userEntity);

        return new AuthUserDTO
        {
            User = user,
            Token = token
        };
    }

    public async Task<AccessTokenDTO> GenerateAccessToken(int userId, string userName, string email)
    {
        var refreshToken = _jwtFactory.GenerateRefreshToken();
        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            UserId = userId
        };
        await _refreshTokenRepository.AddAsync(refreshTokenEntity);

        await UnitOfWork.SaveAsync();

        var accessToken = await _jwtFactory.GenerateAccessToken(userId, userName, email);
        
        return new AccessTokenDTO(accessToken, refreshToken);
    }

    public async Task<AccessTokenDTO> RefreshToken(RefreshTokenDTO dto)
    {
        var userId = _jwtFactory.GetUserIdFromToken(dto.AccessToken, dto.SigningKey);
        var userEntity = await _userRepository.GetByIdAsync(userId);

        if (userEntity == null)
        {
            throw new NotFoundException(nameof(User), userId);
        }

        var rToken = await _refreshTokenRepository.GetByTokenAndUserIdAsync(dto.RefreshToken, userId);
        if (rToken == null)
        {
            throw new InvalidTokenException("refresh");
        }

        if (!rToken.IsActive)
        {
            throw new ExpiredRefreshTokenException();
        }

        var jwtToken = await _jwtFactory.GenerateAccessToken(userEntity.Id, userEntity.Username, userEntity.Email);
        var refreshToken = _jwtFactory.GenerateRefreshToken();

        _refreshTokenRepository.Delete(rToken);
        var newRefreshToken = new RefreshToken
        {
            Token = refreshToken,
            UserId = userEntity.Id
        };

        await _refreshTokenRepository.AddAsync(newRefreshToken);

        await UnitOfWork.SaveAsync();

        return new AccessTokenDTO(jwtToken, refreshToken);
    }

    public async Task RevokeRefreshToken(string refreshToken)
    {
        var userId = _userDataGetter.CurrentUserId;
        var rToken = await _refreshTokenRepository.GetByTokenAndUserIdAsync(refreshToken, userId);

        if (rToken == null)
        {
            throw new InvalidTokenException("refresh");
        }

        _refreshTokenRepository.Delete(rToken);
        await UnitOfWork.SaveAsync();
    }
}