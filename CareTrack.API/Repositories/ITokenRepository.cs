using Microsoft.AspNetCore.Identity;

namespace CareTrack.API.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
