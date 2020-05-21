using Microsoft.AspNetCore.Mvc;
using Products.Controllers;
using Products.Domain;
using Products.Services.Hash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Products.Tests
{
    public class UsersApiTests : IClassFixture<SharedDatabase>
    {
        public SharedDatabase Context { get; set; }
        public UsersApiTests(SharedDatabase sharedDatabase)
        {
            Context = sharedDatabase;
        }

        [Fact]
        public async void Try_Add_Wrong_User()
        {
            using (var context = Context.CreateContext())
            {
                var controller = new UsersController(context, new HashService());
                var users = await controller.GetUsers();
                var previousCount = users.Value.Count();
                var user = new User { Username = null };
                await controller.PostUser(user);
                users = await controller.GetUsers();
                Assert.Equal(previousCount, users.Value.Count());
            }
        }

        [Fact]
        public async void Try_Add_Right_User()
        {
            using (var context = Context.CreateContext())
            {
                var controller = new UsersController(context, new HashService());
                var users = await controller.GetUsers();
                var previousCount = users.Value.Count();
                var user = new User { Username = "username", Password = "password", Role = "user" };
                await controller.PostUser(user);
                users = await controller.GetUsers();
                Assert.Equal(previousCount + 1, users.Value.Count());
            }
        }

        [Fact]
        public void Try_Authorize_With_Wrong_Data()
        {
            using (var context = Context.CreateContext())
            {
                var controller = new UsersController(context, new HashService());
                var user = new User { Role = "user", Password = "ertdrmoptgdmlp", Username = "haHAa" };
                var result = controller.TryLogin(user);
                var code = (StatusCodeResult)result;
                Assert.Equal(401, code.StatusCode);
            }
        }

        [Fact]
        public async void Try_Authorize_With_Right_Data()
        {
            using (var context = Context.CreateContext())
            {
                var controller = new UsersController(context, new HashService());
                var user = new User { Role = "user", Password = "123456", Username = "rightuser" };
                await controller.PostUser(user);
                var result = controller.TryLogin(user);
                var code = (StatusCodeResult)result;
                Assert.Equal(202, code.StatusCode);
            }
        }
    }
}
