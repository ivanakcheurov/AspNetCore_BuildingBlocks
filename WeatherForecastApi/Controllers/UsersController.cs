using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WeatherForecastApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UsersController(ILogger<UsersController> logger, IRepository repository, ITokenService tokenService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<User>> GetUsers()
    {
        return Ok(repository.GetUsers());
    }

    [HttpGet]
    [Route("{id:guid}")]

    public ActionResult<User> GetUser(Guid id)
    {
        var user = repository.GetUser(id);
        if (user is null)
        {
            return NotFound();
        }

        return user;
    }

    [AllowAnonymous]
    [HttpPost]
    public ActionResult<SignedUpUser> Signup(SigningUpUser signingUpUser)
    {
        if (repository.GetUser(signingUpUser.UserName) is not null)
        {
            return BadRequest($"User named {signingUpUser.UserName} already exists");
        }
        using var hmac = new HMACSHA512();
        var passBinary = Encoding.UTF8.GetBytes(signingUpUser.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = signingUpUser.UserName,
            PasswordHash = hmac.ComputeHash(passBinary),
            PasswordSalt = hmac.Key,
        };

        repository.AddUser(user);

        return Created($"users/{user.Id}", new SignedUpUser { Id = user.Id, UserName = user.UserName });
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public ActionResult<SignedInUser> Login(SigningInUser signingInUser)
    {
        var user = repository.GetUser(signingInUser.UserName);
        if (user is null) { return Unauthorized("Cannot find such a username"); }

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var passBinary = Encoding.UTF8.GetBytes(signingInUser.Password);
        var hash = hmac.ComputeHash(passBinary);
        
        if (!hash.SequenceEqual(user.PasswordHash)) {
            return Unauthorized("Password doesn't match");
        }

        var token = tokenService.GenerateToken(user);
        return new SignedInUser{ UserName = user.UserName, Token = token};
    }
}