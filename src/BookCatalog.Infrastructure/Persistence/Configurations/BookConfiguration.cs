using BookCatalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookCatalog.Infrastructure.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.Title).IsRequired().HasMaxLength(200);
        b.Property(x => x.Isbn).IsRequired().HasMaxLength(20);
        b.HasIndex(x => x.Isbn).IsUnique();
        b.Property(x => x.Price).HasColumnType("decimal(18,2)");
        b.HasOne(x => x.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
        b.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Book_PublicationYear",
                "[PublicationYear] >= 1500 AND [PublicationYear] <= 2100");
            t.HasCheckConstraint("CK_Book_Price_NonNegative",
                "[Price] >= 0");
        });
    }
}