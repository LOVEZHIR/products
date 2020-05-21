using Microsoft.EntityFrameworkCore;
using Npgsql;
using Products.Domain;
using Products.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Products.Tests
{
    public class SharedDatabase
    {
        private static readonly object _lock = new object();
        private static bool _databaseInitialized;

        public SharedDatabase()
        {
            Connection = new NpgsqlConnection(@"Host=localhost;Port=5432;Database=products;Username=postgres;Password=1234QWER+");
            Seed();
            Connection.Open();
        }

        public DbConnection Connection { get; }

        public ProductsDbContext CreateContext(DbTransaction transaction = null)
        {
            var context = new ProductsDbContext(new DbContextOptionsBuilder<ProductsDbContext>().UseNpgsql(Connection).Options);

            if (transaction != null)
            {
                context.Database.UseTransaction(transaction);
            }

            return context;
        }

        private void Seed()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        var firstRoom = new Room { Name = "first", Creator = "someCreator" };
                        var secondRoom = new Room { Name = "second", Creator = "someCreator" };
                        var thirdRoom = new Room { Name = "third", Creator = "someAnotherCreator" };

                        var firstRoomUser = new RoomUser { RoomId = 3, Username = "someCreator" };

                        context.AddRange(firstRoom, secondRoom, thirdRoom, firstRoomUser);

                        context.SaveChanges();
                    }

                    _databaseInitialized = true;
                }
            }
        }

        public void Dispose() => Connection.Dispose();
    }
}
