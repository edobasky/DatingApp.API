using DatingApp.API.Entities;
using DatingApp.API.Repository;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class UserRepository(DataContext context) : IUserRepository
    {
        public async Task<AppUser?> GetUserByIdAsync(int id)
        {
            return await context.AppUsers.FindAsync(id);
        }

        public async Task<AppUser?> GetUserByUsernameAsync(string username)
        {
            return await context.AppUsers.SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await context.AppUsers.ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            context.Entry(user).State = EntityState.Modified;
        }
    }
}
