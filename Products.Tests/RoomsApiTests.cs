using Microsoft.EntityFrameworkCore;
using Products.Controllers;
using Products.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Products.Tests
{
    public class RoomsApiTests : IClassFixture<SharedDatabase>
    {
        public SharedDatabase Context { get; set; }
        public RoomsApiTests(SharedDatabase sharedDatabase)
        {
            Context = sharedDatabase;
        }

        [Fact]
        public async void Try_Get_Rooms()
        {
            using (var context = Context.CreateContext())
            {
                var controller = new RoomsController(context);
                var rooms = context.Rooms.ToList();
                var gotRooms = await controller.GetRooms();
                Assert.Equal(rooms, gotRooms.Value);
            }
        }

        [Fact]
        public async void Try_Get_Room()
        {
            using (var context = Context.CreateContext())
            {
                var controller = new RoomsController(context);
                var room = context.Rooms.Where(x => x.Id == 2).FirstOrDefault();
                var gotRoom = await controller.GetRoom(room.Id);
                Assert.Equal(room, gotRoom.Value);
            }
        }

        [Fact]
        public async void Try_Put_Room()
        {
            using (var context = Context.CreateContext())
            {
                var controller = new RoomsController(context);
                var room = context.Rooms.Where(x => x.Id == 1).FirstOrDefault();
                room.Creator = "someNewCreator";
                await controller.PutRoom(room.Id, room);
                var gotRoom = await controller.GetRoom(room.Id);
                Assert.Equal(room, gotRoom.Value);
            }
        }

        [Fact]
        public async void Try_Post_Wrong_Room()
        {
            using (var context = Context.CreateContext())
            {
                var previousCount = context.Rooms.Count();
                var controller = new RoomsController(context);
                var someWrongRoom = new Room { Creator = "", Name = null };
                await controller.PostRoom(someWrongRoom);
                Assert.Equal(previousCount, context.Rooms.Count());
            }
        }

        [Fact]
        public async void Try_Post_Okey_Room()
        {
            using (var context = Context.CreateContext())
            {
                var previousCount = context.Rooms.Count();
                var controller = new RoomsController(context);
                var someWrongRoom = new Room { Creator = "SomeCreator", Name = "SomeName" };
                await controller.PostRoom(someWrongRoom);
                Assert.Equal(previousCount + 1, context.Rooms.Count());
            }
        }

        [Fact]
        public async void Try_Delete_Room()
        {
            using (var context = Context.CreateContext())
            {
                var previousCount = context.Rooms.Count();
                var controller = new RoomsController(context);
                await controller.DeleteRoom(2);
                Assert.Equal(previousCount - 1, context.Rooms.Count());
            }
        }

        [Fact]
        public async void Try_Get_Rooms_For_Users()
        {
            using (var context = Context.CreateContext())
            {
                var username = "someCreator";
                var controller = new RoomsController(context);
                var userRooms = await controller.GetRoomsForUser(username);
                var rooms = await context.RoomsUsers.Where(x => x.Username == username).Select(x => x.RoomId).ToListAsync();
                var expected = await context.Rooms.Where(x => rooms.Contains(x.Id)).ToListAsync();
                Assert.Equal(userRooms.Value, expected);
            }
        }
    }
}
