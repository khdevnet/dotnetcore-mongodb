using Books.Data.Sql.Entity;
using Microsoft.EntityFrameworkCore;

namespace Books.Data.Sql.Database
{
    public class BooksSqlDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"Host=127.0.0.1;Database=book_library;Username=postgres;Password=123456");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildBook(modelBuilder);
        }

        private static void BuildBook(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().ToTable("book");

            modelBuilder.Entity<Book>()
                .Property(b => b.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            modelBuilder.Entity<Book>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Book>()
                .Property(p => p.Title)
                .HasColumnName("title")
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Book>()
                .Property(p => p.Path)
                .HasColumnName("path")
                .IsRequired();
        }
    }
}
