using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Products.Domain;
using Products.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Products.Hubs
{
    public class ProductsHub:Hub
    {
        private readonly ProductsDbContext _context;
        public ProductsHub(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task TryEnterUsingPassword(int roomId, string username, string password)
        {
            var room = await _context.Rooms.FindAsync(roomId);

            if (room!=null && room.Password == password)
            {
                await _context.RoomsUsers.AddAsync(new Domain.RoomUser { RoomId = roomId, Username = username });
                await _context.SaveChangesAsync();
                await Clients.Caller.SendAsync("EnterRoom", roomId, username);
            }
            else
            {
                await Clients.Caller.SendAsync("WrongPassword");
            }
        }

        public async Task TryEnter(int roomId, string username)
        {
            var roomUser = await _context.RoomsUsers.FindAsync(roomId, username);
            if (roomUser != null)
            {
                await Clients.Caller.SendAsync("EnterRoom", roomId, username);
                return;
            }
            else
            {
                await Clients.Caller.SendAsync("EnterPassword");
            }
        }

        public async Task AddToGroup(int roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
        }

        public async Task SendMessageToGroup(int roomId, string username, string message)
        {
            await Clients.Group(roomId.ToString()).SendAsync("GroupMessage", username, message);
            await _context.RoomsMessages.AddAsync(new RoomMessage { Message =$"{username}:{message}", RoomId = roomId });
            await _context.SaveChangesAsync();
        }

        public async Task AddNewProduct(int roomId, string product)
        {
            await Clients.Group(roomId.ToString()).SendAsync("AddNewProduct", product);
            await _context.RoomsProducts.AddAsync(new Domain.RoomProduct { Product = product, RoomId = roomId, IsChecked = false }) ;
            await _context.SaveChangesAsync();
        }
        
        public async Task GetData(int roomId)
        {
            var users = _context.RoomsUsers.Where(x => x.RoomId == roomId).Select(x=>x.Username).ToList();
            var messages = _context.RoomsMessages.Where(x => x.RoomId == roomId).Select(x => x.Message).ToList();
            var products = _context.RoomsProducts.Where(x => x.RoomId == roomId).Select(x => new ProductChecked
            {
                Product=x.Product,
                Checked=x.IsChecked
            }).ToList();

            var jsonUsers = JsonSerializer.Serialize(users);
            var jsonMessages = JsonSerializer.Serialize(messages);
            var jsonProducts = JsonSerializer.Serialize(products);

            await Clients.Group(roomId.ToString()).SendAsync("Data", jsonUsers, jsonMessages, jsonProducts);
        }

        public async Task CheckProduct(int roomId, string product)
        {
            var item = _context.RoomsProducts.Where(x => x.RoomId == roomId && x.Product == product).FirstOrDefault();
            if (item != null)
            {
                item.IsChecked = true;
                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearProducts(int roomId)
        {
            var needToClear=_context.RoomsProducts.Where(x => x.RoomId == roomId);
            _context.RoomsProducts.RemoveRange(needToClear);
            await _context.SaveChangesAsync();
        }
    }
}
