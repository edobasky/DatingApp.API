using DatingApp.API.DTOs;
using DatingApp.API.Entities;

namespace DatingApp.API.Repository
{
    public interface ILikesRepository
    {
        Task<UserLike?> GetUserLike(int sourceUserId, int targetUserId); // individual user like

        Task<IEnumerable<MemberDto>> GetUserLikes(string predicate, int userId); // gives option to users they like and that have like them and mutual likes
        Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId); // users a client have liked
        void DeleteLike(UserLike like);
        void AddLike(UserLike like);

        Task<bool> SaveChanges();
    }
}
