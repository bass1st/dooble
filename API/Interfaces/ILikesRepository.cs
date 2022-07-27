namespace API.Interfaces;

public interface ILikesRepository
{
    Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);
    Task<AppUser> GetUserWithLikes(int userId);
    Task<PagedList<LikesDto>> GetUserLikes(LikesParams likesParams);
}