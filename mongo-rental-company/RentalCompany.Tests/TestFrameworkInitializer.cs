using MongoDB.Bson.IO;
using RentalCompany.Domain.Mapper;
using Xunit.Abstractions;
using Xunit.Sdk;

[assembly: Xunit.TestFramework("RentalCompany.Tests.TestFrameworkInitializer", "RentalCompany.Tests")]
namespace RentalCompany.Tests
{
    public class TestFrameworkInitializer : XunitTestFramework
    {
        public TestFrameworkInitializer(IMessageSink messageSink)
          : base(messageSink)
        {
            JsonWriterSettings.Defaults.Indent = true;
            MongoDbInitializer.Init();
        }

        public new void Dispose()
        {
            // Place tear down code here
            base.Dispose();
        }
    }
}
