using System.Collections.Generic;
using MongoDB.Bson;
using Xunit;
using Xunit.Abstractions;

namespace RentalCompany.Tests
{
    public class PocoTests
    {
        private readonly ITestOutputHelper output;

        public PocoTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Automatic()
        {
            var d = new Person()
            {
                FirstName = "Anton",
                Age = 15,
                Address = new[]
                 {
                     "Address1"
                 },
                Contact = new Contact
                {
                    Email = "test@gmail.com"
                }
            };

            output.WriteLine(d.ToJson());
        }

        public class Person
        {
            public string FirstName { get; set; }
            public int Age { get; set; }
            public IEnumerable<string> Address { get; set; }
            public Contact Contact { get; set; }
        }

        public class Contact
        {
            public string Email { get; set; }
        }
    }
}
