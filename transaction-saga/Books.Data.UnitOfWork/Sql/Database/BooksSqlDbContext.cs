using System;
using System.Collections.Generic;
using Books.Core;
using Books.Domain;
using Books.Domain.Extensibility.Provider;
using Microsoft.EntityFrameworkCore;

namespace Books.Data.UnitOfWork.Sql.Database
{
    public class BooksSqlDbContext : DbContext, ITransactionDbContext
    {
        private readonly IBookFilePathProvider bookFilePathProvider;

        public BooksSqlDbContext(DbContextOptions<BooksSqlDbContext> options,
            IBookFilePathProvider bookFilePathProvider)
            : base(options)
        {
            this.bookFilePathProvider = bookFilePathProvider;
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<CreateBookSaga> CreateBookSagas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildBook(modelBuilder);
            SeedBooks(modelBuilder);
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

        private void SeedBooks(ModelBuilder modelBuilder)
        {
            var books = new List<Book>
            {   CreateBook("A Space Odissey","Clarke, Arthur C"),
                CreateBook("A tenderfoot in space","Heinlein, Robert Anson"),
                CreateBook("A Hole in Space","Niven, Larry"),
            };

            books.ForEach(b => modelBuilder.Entity<Book>().HasData(b));
        }

        private Book CreateBook(string bookName, string authorName)
        {
            return new Book { Id = Guid.NewGuid(), Title = bookName, Author = authorName, Path = GetBookPath(bookName, authorName) };
        }

        private string GetBookPath(string fileName, string authorName) => bookFilePathProvider.GetRelativePath(fileName, authorName);

        public ITransaction CreateTransaction()
        {
            return new SqlTransaction(this);
        }
    }
}
