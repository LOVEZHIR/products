using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Services.Hash
{
    public interface IHashService
    {
        public byte[] GetHash(string inputString);
        public string GetHashString(string inputString);
    }
}
