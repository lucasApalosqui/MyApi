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
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(x => x.Name)
                .HasColumnName("Name")
                .HasColumnType("VARCHAR")
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(x => x.Slug)
                .HasColumnName("Slug")
                .HasColumnType("VARCHAR")
                .HasMaxLength(80)
                .IsRequired();

            builder.HasIndex(x => x.Slug, "IX_Role_Slug")
                .IsUnique();
        }
    }
}
