using System;
using System.Collections.Generic;
using System.Text;

namespace Products.Infrastructure.Repositories
{
    public interface IRepository<T> where T : class
    {
        List<T> GetList();
        T GetById(long id);
        void Create(T item);
        void Update(T item);
        void DeleteById(long id);
        void Save();
    }
}
