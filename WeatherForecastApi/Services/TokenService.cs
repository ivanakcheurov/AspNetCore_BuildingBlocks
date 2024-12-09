using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public interface ITokenService
{
    string GenerateToken(User user);

}
public class TokenService(IConfiguration conf) : ITokenService
{
    public string GenerateToken(User user)
    {
        var signatureKey = conf["PrivateSignatureKey"] ?? throw new Exception("Cannot find PrivateSignatureKey");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signatureKey));

        var claims = new [] {
            new Claim(ClaimTypes.NameIdentifier, user.UserName),
        };

        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(12), // e.g. standard expiration in AWS
            SigningCredentials = signingCredentials,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        var serializedToken = tokenHandler.WriteToken(token);

        return serializedToken;
    }

    public static TokenValidationParameters GetTokenValidationParameters(IConfiguration conf) => new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["PrivateSignatureKey"] ?? throw new Exception("Cannot find PrivateSignatureKey"))),
        ValidateIssuer = false,
        ValidateAudience = false
    };
}