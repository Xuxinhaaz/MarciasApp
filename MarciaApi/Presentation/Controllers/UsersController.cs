using MarciaApi.Infrastructure.Services.Email;
using Microsoft.AspNetCore.Mvc;

namespace MarciaApi.Presentation.Controllers;

[ApiController]
public class UsersController : ControllerBase
{
    private readonly IEmailSender _emailSender;

    public UsersController(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    [HttpGet("/")]
    public async Task<IActionResult> GetUsers()
    {
        return new OkObjectResult("Ola Mundo!");
    }

    [HttpPost("/Email")]
    public async Task<IActionResult> TestEmail()
    {
        var email = "joojjunu@gmail.com";
        
        await _emailSender.SendEmailAsync(email, "Marcia Foda", "caio's Dick is the smallest i've ever seen!!!!!");
        
        return new OkObjectResult(new
        {
            ok = "We send it"
        });
    }
}