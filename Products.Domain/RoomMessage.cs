using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Domain
{
    public class RoomMessage
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string Message { get; set; }
    }
}
