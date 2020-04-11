using MongoDB.Migrations.Tool.Books;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;
using Xunit.Sdk;

[assembly: Xunit.TestFramework("MongoDB.Migrations.Tool.TestFrameworkInitializer", "MongoDB.Migrations.Tool")]

namespace MongoDB.Migrations.Tool
{
    public class TestFrameworkInitializer : XunitTestFramework
    {
        public TestFrameworkInitializer(IMessageSink messageSink)
          : base(messageSink)
        {
            BooksNoSqlInitialzer.Init();
        }

        public new void Dispose()
        {
            // Place tear down code here
            base.Dispose();
        }
    }
}
