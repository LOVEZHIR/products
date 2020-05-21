using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Domain
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Creator { get; set; }
        public string Password { get; set; }
    }
}
