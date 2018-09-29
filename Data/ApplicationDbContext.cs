using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCore_Template.Data.Model;

namespace NetCore_Template.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApplicationDbContext(DbContextOptionsBuilder<ApplicationDbContext> options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("User").HasKey(x => x.Id);
            //builder.Entity<ApplicationUser>().HasIndex("NormalizedEmail").HasName("EmailIndex").IsUnique(false);
            builder.Entity<ApplicationRole>().ToTable("Role").HasKey(x => x.Id);
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRole").HasKey(x => new { x.UserId, x.RoleId });
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaim").HasKey(x => x.Id);
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogin").HasKey(x => new { x.ProviderKey, x.LoginProvider });
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaim").HasKey(x => x.Id);
            builder.Entity<IdentityUserToken<int>>().ToTable("UserToken").HasKey(x => x.UserId);
        }
    }
}
