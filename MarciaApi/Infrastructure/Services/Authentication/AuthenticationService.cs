using AutoMapper;
using MarciaApi.Application.Services.Authentication;
using MarciaApi.Application.Services.Email;
using MarciaApi.Domain.Repository.User;
using MarciaApi.Presentation.DTOs.User;
using MarciaApi.Presentation.ViewModel.User;
using Microsoft.AspNetCore.Identity;

namespace MarciaApi.Infrastructure.Services.Authentication;

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

    public async Task<UserModelDto> SignUpAsync(UserViewModel model)
    {
        try
        {
            var anyUserSignedUp = await _userRepository.AnyUserWithSameEmailProvided(model.Email);

            if (anyUserSignedUp)
            {
                await _emailSender.SendEmailAsync(model.Email, 
                    "Marmitaria da Marcia",
                    "caio's Dick is the smallest i've ever seen!!!!!");

                var userFound = await _userRepository.FindByEmailAsync(model.Email);
                var tokenJwt = await _jwtService.Generate(model);
                
                var userFoundDto = _mapper.Map<UserModelDto>(userFound);
                userFoundDto.JwtToken = tokenJwt;
                
                return userFoundDto;
            }   
            
            var token = await _jwtService.Generate(model);
            var newUser = _userRepository.Generate(model);
            
            await _emailSender.SendEmailAsync(model.Email, 
                "Marmitaria da Marcia",
                "caio's Dick is the smallest i've ever seen!!!!!");
            
            var dto = _mapper.Map<UserModelDto>(newUser);
            dto.JwtToken = token;

            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during sign-up");
            throw;
        }
    }
}