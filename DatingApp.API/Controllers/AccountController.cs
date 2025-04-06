using System.Security.Cryptography;
using System.Text;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(DataContext context) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
        {
            if (await UserExist(registerDto.Username)) return BadRequest("User already exist with username");
            using var hmac = new HMACSHA512(); // Self dispose once class is out of scope

            var user = new AppUser
            {
                UserName = registerDto.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            context.AppUsers.Add(user);
            await context.SaveChangesAsync();

            return user;
        }

        [HttpPost("login")] 
        public async Task<ActionResult<AppUser>> Login(LoginDto loginDto)
        {
            var user = await context.AppUsers
                .AsNoTracking()
                .FirstOrDefaultAsync( x => x.UserName.ToLower() == loginDto.Username.ToLower());

            if (user == null) return Unauthorized("Inavlid login username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }

            return user;

        }

        private async Task<bool> UserExist(string username)
        {
            return await context.AppUsers.AsNoTracking().AnyAsync(x => x.UserName.ToLower() == username.ToLower());
        }
    }
}
