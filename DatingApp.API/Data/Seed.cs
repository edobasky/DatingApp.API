using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using DatingApp.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext dataContext)
        {
            if (await dataContext.AppUsers.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData,options);
            if (users == null) return;  

            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();

                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
                user.PasswordSalt = hmac.Key;

                await dataContext.AppUsers.AddAsync(user);  
            }
            await dataContext.SaveChangesAsync();
            
        }
    }
}
