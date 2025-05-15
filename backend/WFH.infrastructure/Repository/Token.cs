using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WorkFromHome.Application.Common.Repository;

namespace WFH.infrastructure.Repository
{
    public class Token : IToken
    {
        private readonly IConfiguration _configuration;

        public Token(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public string GenerateRefreshToken()
        {
            try
            {
                var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(300));
                return refreshToken;
            }
            catch (Exception )
            {
                throw;
            }
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            try
            {
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!);
                var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: credentials);

                var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
                return accessToken;
            }
            catch (Exception )
            {
                throw;
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
        {
            try
            {
                var tokenValdiationParameter = new TokenValidationParameters
                {
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(accessToken, tokenValdiationParameter, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;
               
                return principal;
            }
            catch (Exception )
            {
                throw;
            }

        }
    }
}
