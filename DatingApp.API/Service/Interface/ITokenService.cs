using DatingApp.API.Entities;

namespace DatingApp.API.Service.Interface
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
    }
}
