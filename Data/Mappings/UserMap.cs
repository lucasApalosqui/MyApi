using BlogAspNet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAspNet.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(x => x.Name)
                .HasColumnName("Name")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(x => x.Email)
                .HasColumnName("Email")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.PasswordHash)
                .HasColumnName("PasswordHash")
                .HasColumnType("VARCHAR")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Bio)
                .HasColumnName("Bio")
                .HasColumnType("TEXT")
                .IsRequired(false);

            builder.Property(x => x.Image)
                .HasColumnName("Image")
                .HasColumnType("VARCHAR")
                .HasMaxLength(2000)
                .IsRequired(false);

            builder.Property(x => x.Slug)
                .HasColumnName("Slug")
                .HasColumnType("VARCHAR")
                .HasMaxLength(80)
                .IsRequired();

            builder.HasIndex(x => x.Slug, "IX_User_Slug")
                .IsUnique();

            builder.HasIndex(x => x.Email, "IX_User_Email")
                .IsUnique();

            builder.HasMany(x => x.Roles)
                .WithMany(x => x.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                        user => user.HasOne<Role>()
                            .WithMany()
                            .HasForeignKey("RoleId")
                            .HasConstraintName("FK_UserRole_RoleId")
                            .OnDelete(DeleteBehavior.Cascade),
                        role => role.HasOne<User>()
                            .WithMany()
                            .HasForeignKey("UserId")
                            .HasConstraintName("FK_UserRole_UserId")
                            .OnDelete(DeleteBehavior.Cascade)
                            
                );
        }
    }
}
