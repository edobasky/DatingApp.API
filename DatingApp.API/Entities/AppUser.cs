namespace DatingApp.API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required byte[] PasswordHash { get; set; } // one way encryption

        public required byte[] PasswordSalt { get; set; } // alter the hash further

    }
}
