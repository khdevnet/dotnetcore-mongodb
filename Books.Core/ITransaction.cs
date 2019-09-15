using System;
using System.Collections.Generic;
using System.Text;

namespace Books.Core
{
    public interface ITransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
