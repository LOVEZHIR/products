using Microsoft.EntityFrameworkCore;
using Products.Domain;
using Products.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Products.Infrastructure.Repositories
{
    public class RoomRepository : IRepository<Room>
    {
        private readonly ProductsDbContext _context;
        public RoomRepository(ProductsDbContext context)
        {
            _context = context;
        }

        public void Create(Room item)
        {
            _context.Rooms.Add(item);
            Save();
        }

        public void DeleteById(long id)
        {
            var item = GetById(id);
            if (item != null)
                _context.Rooms.Remove(item);
            Save();
        }

        public Room GetById(long id)
        {
            return _context.Rooms.Find(id);
        }

        public List<Room> GetList()
        {
            return _context.Rooms.ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Room item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }
    }
}
