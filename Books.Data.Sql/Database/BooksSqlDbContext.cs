using Books.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Books.Data.Sql.Database
{
    public class BooksSqlDbContext : DbContext
    {
        DbSet<Book> Books { get; set; }
    }
}
