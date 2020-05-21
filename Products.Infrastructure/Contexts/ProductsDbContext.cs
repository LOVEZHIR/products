using Microsoft.EntityFrameworkCore;
using Products.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Infrastructure.Contexts
{
    public class ProductsDbContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomUser> RoomsUsers { get; set; }
        public DbSet<RoomProduct> RoomsProducts { get; set; }
        public DbSet<RoomMessage> RoomsMessages { get; set; }
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options):base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();

            modelBuilder.Entity<RoomUser>().HasKey(x => new { x.RoomId, x.Username });
            modelBuilder.Entity<RoomProduct>().HasKey(x => new { x.RoomId, x.Product });
        }
    }
}
