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
        private readonly DataContext _db;

        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailsRepository OrderDetails { get; private set; }
        public IAppUSerRepository AppUSer { get; private set; }

        public UnitOfWork(DataContext db, ICategoryRepository category, IProductRepository product, ICompanyRepository company, IShoppingCartRepository shoppingCart,
            IOrderHeaderRepository orderHeader, IOrderDetailsRepository orderDetails,IAppUSerRepository appUSer)
        {
            _db = db;
            Category = category;
            Product = product;
            Company = company;
            ShoppingCart = shoppingCart;
            OrderHeader = orderHeader;
            OrderDetails = orderDetails;
            AppUSer = appUSer;
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
