using AutoMapper;
using ErrorOr;
using MarciaApi.Application.Services.Authentication;
using MarciaApi.Application.Services.Email;
using MarciaApi.Domain.Models;
using MarciaApi.Domain.Repository.User;
using MarciaApi.Presentation.DTOs.User;
using MarciaApi.Presentation.ViewModel.User;

namespace MarciaApi.Infrastructure.Services.Auth.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly IEmailSender _emailSender;

    public AuthenticationService(IUserRepository userRepository, IJwtService jwtService, IMapper mapper, ILogger<AuthenticationService> logger, IEmailSender emailSender)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _mapper = mapper;
        _logger = logger;
        _emailSender = emailSender;
    }

    public async Task<ErrorOr<UserModelDto>> SignUpAsync(UserViewModel model)
    {
            var anyUserSignedUp = await _userRepository.Any(x => x.Email == model.Email);

            if (anyUserSignedUp)
            {
                await _emailSender.SendEmailAsync(model.Email!, 
                    "Marmitaria da Marcia",
                    "OMG IM CUMMING!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

                var userFound = await _userRepository.Get(x => x.Email == model.Email, x => x.Roles);
                var tokenresult = await _jwtService.Generate(userFound);
                if (tokenresult.IsError)
                    return tokenresult.Errors;
                
                var userFoundDto = _mapper.Map<UserModelDto>(userFound);
                userFoundDto.JwtToken = tokenresult.Value;
                
                return userFoundDto;
            }   
            
            ErrorOr<UserModel> ModelResult = await _userRepository.Generate(model);
            if (ModelResult.IsError)
                return ModelResult.Errors;
            ErrorOr<string> tokenResult = await _jwtService.Generate(ModelResult.Value);
            if (tokenResult.IsError)
                return tokenResult.Errors;
        
            await _emailSender.SendEmailAsync(model.Email, 
                "Marmitaria da Marcia",
                "OMG IM CUMMING!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            
            var dto = _mapper.Map<UserModelDto>(tokenResult.Value);
            dto.JwtToken = tokenResult.Value;

            return dto;
    }
}