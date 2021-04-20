using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TutoringApp.Configurations.Auth
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole { Id = "ffbbf0f7-a44b-4507-9638-16f23fe8d45e", ConcurrencyStamp = "aef4be17-59c2-4348-aa62-e3240fa214bf", Name = AppRoles.Student, NormalizedName = AppRoles.Student.ToUpper() },
                new IdentityRole { Id = "6e30ffbf-e691-4e41-9349-0404349a8367", ConcurrencyStamp = "81347000-aeb1-42fb-be76-80d87e0709e0", Name = AppRoles.Admin, NormalizedName = AppRoles.Admin.ToUpper() }
            );
        }
    }
}
