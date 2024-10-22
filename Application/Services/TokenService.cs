using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;
    public TokenService(IConfiguration config)
    {
        _config = config;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
    }

    public string GetToken(User user)
    {
        var cred = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()), new Claim("email", user.Email.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(3),
            SigningCredentials = cred,
            Issuer = _config["JWT:Issuer"],
            Audience = _config["JWT:Audience"]
        };

        

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
