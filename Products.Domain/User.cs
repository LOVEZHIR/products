using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Domain
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int Id { get; set; }
        public string Role { get; set; }
    }
}
