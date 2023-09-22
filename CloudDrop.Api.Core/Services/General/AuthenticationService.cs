using AutoMapper;

using CloudDrop.Api.Core.Constants;
using CloudDrop.Api.Core.Contracts.Repositories;
using CloudDrop.Api.Core.Contracts.Services.General;
using CloudDrop.Api.Core.Entities;
using CloudDrop.Api.Core.Models.Dtos;
using CloudDrop.Api.Core.Models.Requests;
using CloudDrop.Api.Core.Models.Settings;
using CloudDrop.Shared.Models.Requests;
using CloudDrop.Shared.Models.Responses;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using System.Text;

namespace CloudDrop.Api.Core.Services.General;
public class AuthenticationService(IMapper mapper,
    IUserRepository userRepository,
    IConfiguration configuration) : IAuthenticationService
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IConfiguration _configuration = configuration;

    public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest req, CancellationToken cancellation = default)
    {
        UserEntity? user = await _userRepository.GetByUsernameOrEmailAsync(req.Username, cancellation);
        if (user is null || !BCrypt.Net.BCrypt.Verify(req.Password, user.Password))
        {
            return new AuthenticationResponse()
            {
                IsAuthenticated = false,
                Message = "Invalid username or password",
            };
        }

        var accessToken = GetAccessToken(user);
        return new AuthenticationResponse()
        {
            IsAuthenticated = true,
            Message = "Authenticated",
            Data = new AuthenticationDataResponse()
            {
                User = _mapper.Map<UserResponse>(user),
                AccessToken = new AccessTokenResponse()
                {
                    Token = accessToken.Token,
                    ExpiryDate = accessToken.ExpiryDate
                },
            }
        };

    }
    public async Task<UserResponse> RegisterAsync(AddUserRequest req, CancellationToken cancellation = default)
    {
        UserEntity user = _mapper.Map<UserEntity>(req);
        user.Password = BCrypt.Net.BCrypt.HashPassword(req.Password);

        bool isAdded = await _userRepository.AddUserAsync(user, cancellation);
        if (!isAdded)
        {
            throw new Exception("Unable to add User.");
        }

        return _mapper.Map<UserResponse>(user);
    }


    private AuthenticationSettings? GetAuthConfig()
    {
        return _configuration
            .GetSection(AuthenticationSettings.SettingsKey)
            .Get<AuthenticationSettings>();
    }
    private AccessTokenDto GetAccessToken(UserEntity user)
    {
        AuthenticationSettings authConfig = GetAuthConfig()
            ?? throw new Exception("Invalid auth config");

        DateTime expiry = DateTime.UtcNow.Add(authConfig.AccessTokenExpiry);


        var key = Encoding.ASCII.GetBytes(authConfig.SecretKey);

        ClaimsIdentity claims = new(new[]
        {
            new Claim(ClaimsConstant.Id, user.Id.ToString()),
            new Claim(ClaimsConstant.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimsConstant.Email, user.Email),
            new Claim(ClaimsConstant.Jti, Guid.NewGuid().ToString()),
        });

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = expiry,
            Issuer = authConfig.Issuer,
            Audience = authConfig.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AccessTokenDto()
        {
            Token = tokenHandler.WriteToken(token),
            ExpiryDate = expiry,
        };
    }
}
