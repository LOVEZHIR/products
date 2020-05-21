using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Products.Domain;
using Products.Infrastructure.Contexts;
using Products.Services.Hash;

namespace Products.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ProductsDbContext _context;
        private readonly HashService _hashService;

        public UsersController(ProductsDbContext context, HashService hashService)
        {
            _hashService = hashService;
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/Users
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if(String.IsNullOrEmpty(user.Password) || String.IsNullOrEmpty(user.Username))
            {
                return BadRequest();
            }
            if (user.Username == "admin")
                user.Role = "admin";
            else
                user.Role = "user";
            if (_context.Users.Any(x => x.Username == user.Username))
                return StatusCode(400);

            user.Password = _hashService.GetHashString(user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        [Route("getrole")]
        [HttpPost]
        public ActionResult<User> GetRole(User user)
        {
            var rightUser = _context.Users.Where(x => x.Username == user.Username).FirstOrDefault();
            return rightUser;
        }

        [Route("trylogin")]
        [HttpPost]

        public ActionResult TryLogin(User user)
        {
            user.Password = _hashService.GetHashString(user.Password);
            var rightUser = _context.Users.Where(x => x.Username == user.Username).FirstOrDefault();
            if (rightUser != null && rightUser.Password == user.Password)
            {
                return StatusCode(202);
            }
            else
            {
                return StatusCode(401);
            }
        }

    }
}
