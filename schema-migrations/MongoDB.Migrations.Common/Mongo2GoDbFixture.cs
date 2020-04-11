using System;

namespace MongoDB.Migrations.Common
{
    public class MongoDbFixture : IDisposable
    {
        public MongoDbFixture()
        {


        }

        public void Dispose()
        {
            // Do "global" teardown here; Only called once.
        }
    }
}
