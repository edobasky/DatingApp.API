using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DatingApp.API.Entities;
using DatingApp.API.Service.Interface;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Service
{
    public class TokenServiceImp(IConfiguration config) : ITokenService
    {
        public string CreateToken(AppUser appUser)
        {
            var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot access token key from apssetings");
            if (tokenKey.Length < 64) throw new Exception("your token key needs to be longer");
           

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)); // same key is used to encrypt and decrypt, while assymetric has public and private key used in AES Encryption.

            var claim = new List<Claim>
            { 
                new Claim(ClaimTypes.NameIdentifier,appUser.UserName),
            };

            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claim),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds

            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }
    }
}
