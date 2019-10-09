using Books.Data.NoSql;
using Books.Data.Sql;

namespace Books.Data
{
    public class CreateBookDbUnitOfWork
    {
        private CreateBookNoSqlTransaction noSqlTransaction;
        private readonly CreateBookSqlTransaction sqlTransaction;

        public CreateBookDbUnitOfWork(Data.Sql.Entity.Book sqlBook, Data.NoSql.Entity.Book noSqlBook)
        {
            sqlTransaction = new CreateBookSqlTransaction(sqlBook);
            noSqlTransaction = new CreateBookNoSqlTransaction(noSqlBook);
        }

        public void Commit()
        {
            noSqlTransaction.Commit();
            sqlTransaction.Commit();
        }

        public void Rollback()
        {
            noSqlTransaction.Rollback();
            sqlTransaction.Rollback();
        }
    }
}
