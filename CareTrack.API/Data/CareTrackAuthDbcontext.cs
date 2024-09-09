using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CareTrack.API.Data
{
    public class CareTrackAuthDbcontext : IdentityDbContext
    {
        public CareTrackAuthDbcontext(DbContextOptions<CareTrackAuthDbcontext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var adminRoleId = "800bb35d-ef05-46c8-b5f9-7fa483bc7c8a";
            var superAdminRoleId = "123a0079-5ef7-4c36-b1c2-9530186600ad";
            var userRoleId = "53b42cf0-b1b4-4c4d-839f-109b09f2f92f";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()

                } ,

                new IdentityRole
                {

                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId,
                    Name = "Super Admin",
                    NormalizedName = "Super Admin".ToUpper()

                } ,

                 new IdentityRole
                {

                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId,
                    Name = "User",
                    NormalizedName = "User".ToUpper()

                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
