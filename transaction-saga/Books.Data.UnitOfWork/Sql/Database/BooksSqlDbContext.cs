using System;
using System.Collections.Generic;
using Books.Domain;
using Books.Domain.Events;
using Books.Domain.Extensibility.Provider;
using Microsoft.EntityFrameworkCore;

namespace Books.Data.UnitOfWork.Sql.Database
{
    public class BooksSqlDbContext : DbContext
    {
        private readonly IBookFilePathProvider bookFilePathProvider;

        public BooksSqlDbContext(DbContextOptions<BooksSqlDbContext> options,
            IBookFilePathProvider bookFilePathProvider)
            : base(options)
        {
            this.bookFilePathProvider = bookFilePathProvider;
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<SagaEvent> SagaEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildBook(modelBuilder);
            BuildSagaEvents(modelBuilder);
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
                .Property(p => p.Status)
                .HasColumnName("status")
                .IsRequired();

            modelBuilder.Entity<Book>()
                .Property(p => p.Path)
                .HasColumnName("path")
                .IsRequired();
        }

        private static void BuildSagaEvents(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SagaEvent>().ToTable("saga_event");

            modelBuilder.Entity<SagaEvent>()
                .Property(b => b.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            modelBuilder.Entity<SagaEvent>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<SagaEvent>()
                .Property(p => p.Data)
                .HasColumnName("data")
                .IsRequired();

            modelBuilder.Entity<SagaEvent>()
                .Property(p => p.Success)
                .HasColumnName("success")
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
            return new Book
            {
                Id = Guid.NewGuid(),
                Title = bookName,
                Author = authorName,
                Status = BookStatus.FileSaved,
                Path = GetBookPath(bookName, authorName)
            };
        }

        private string GetBookPath(string fileName, string authorName) => bookFilePathProvider.GetRelativePath(fileName, authorName);
    }
}
