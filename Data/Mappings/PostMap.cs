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
    public class PostMap : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Post");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(x => x.Title)
                .HasColumnName("Title")
                .HasColumnType("VARCHAR")
                .HasMaxLength(160)
                .IsRequired();

            builder.Property(x => x.Summary)
                .HasColumnName("Summary")
                .HasColumnType("VARCHAR")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Body)
                .HasColumnName("Body")
                .HasColumnType("TEXT")
                .IsRequired();

            builder.Property(x => x.Slug)
                .HasColumnName("Slug")
                .HasColumnType("VARCHAR")
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(x => x.CreateDate)
                .HasColumnName("CreateDate")
                .HasColumnType("DATETIME")
                .HasDefaultValue(DateTime.Now.ToUniversalTime())
                //.HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.Property(x => x.LastUpdateDate)
                .HasColumnName("LastUpdateDate")
                .HasColumnType("DATETIME")
                .HasDefaultValue(DateTime.Now.ToUniversalTime())
                //.HasDefaultValueSql("GETDATE()")
                .IsRequired();


            builder.HasIndex(x => x.Slug, "IX_Post_Slug")
                .IsUnique();

            // Relacionamento

            builder.HasOne(x => x.Author)
                .WithMany(x => x.Posts)
                .HasConstraintName("FK_Post_Author");

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Posts)
                .HasConstraintName("FK_Post_Category");

            builder.HasMany(x => x.Tags)
                .WithMany(x => x.Posts)
                .UsingEntity<Dictionary<string, object>>(
                    "PostTag",
                    post => post.HasOne<Tag>()
                        .WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK_PostTag_TagId")
                        .OnDelete(DeleteBehavior.Cascade),
                    tag => tag.HasOne<Post>()
                        .WithMany()
                        .HasForeignKey("PostId")
                        .HasConstraintName("FK_PostTag_PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                );

        }
    }
}
