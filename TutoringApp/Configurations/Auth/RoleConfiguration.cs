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
                new IdentityRole { Name = AppRoles.Student, NormalizedName = AppRoles.Student.ToUpper() },
                new IdentityRole { Name = AppRoles.Tutor, NormalizedName = AppRoles.Tutor.ToUpper() },
                new IdentityRole { Name = AppRoles.Admin, NormalizedName = AppRoles.Admin.ToUpper() },
                new IdentityRole { Name = AppRoles.Lecturer, NormalizedName = AppRoles.Lecturer.ToUpper() }
            );
        }
    }
}
