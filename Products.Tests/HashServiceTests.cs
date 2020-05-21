using Products.Services.Hash;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Products.Tests
{
    public class HashServiceTests : IClassFixture<SharedDatabase>
    {
        public SharedDatabase Context { get; set; }
        public HashServiceTests(SharedDatabase sharedDatabase)
        {
            Context = sharedDatabase;
        }

        [Fact]
        public void Try_Get_Another_String_From_HashString()
        {
            var service = new HashService();
            var someRandomString = new Random().Next().ToString();
            var gotString = service.GetHashString(someRandomString);
            Assert.NotEqual(someRandomString, gotString);
        }
    }
}
