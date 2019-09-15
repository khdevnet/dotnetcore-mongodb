namespace Books.Data.NoSql.Database
{
    public class BooksNoSqlDbContextSettings : IBookNoSqlDbContextSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IBookNoSqlDbContextSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
