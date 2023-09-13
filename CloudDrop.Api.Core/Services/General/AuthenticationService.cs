using AutoMapper;

using CloudDrop.Api.Core.Contracts.Services.General;
using CloudDrop.Api.Core.Models.Requests;
using CloudDrop.Api.Core.Models.Responses;

namespace CloudDrop.Api.Core.Services.General;
public class AuthenticationService : IAuthenticationService
{
    private readonly IMapper _mapper;

    public AuthenticationService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest req, CancellationToken cancellation = default)
    {
        //UserEntity? user = await _userRepository.GetByEmailAsync(req.Email, cancellation);
        //if (user is null || !BCrypt.Net.BCrypt.Verify(req.Password, user.Password))
        //{
        //    return new AuthenticationResponse()
        //    {
        //        IsAuthenticated = false,
        //        Message = "Invalid username or password",
        //    };
        //}

        //var accessToken = GetAccessToken(user);
        //return new AuthenticationResponse()
        //{
        //    IsAuthenticated = true,
        //    Message = "Authenticated",
        //    Data = new AuthenticationDataResponse()
        //    {
        //        User = _mapper.Map<UserResponse>(user),
        //        AccessToken = new AccessTokenResponse()
        //        {
        //            Token = accessToken.Token,
        //            ExpiryDate = accessToken.ExpiryDate
        //        },
        //    }
        //};

        return new AuthenticationResponse()
        {
            Message = "Invalid username or password",
            IsAuthenticated = false,
        };

    }
}
