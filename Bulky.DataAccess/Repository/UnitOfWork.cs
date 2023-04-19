using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DtatContext _db;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }

        public UnitOfWork(DtatContext db,ICategoryRepository category,IProductRepository product)
        {
            _db = db;
            Category = category;
            Product = product;
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
