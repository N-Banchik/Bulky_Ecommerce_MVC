using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class CompanyRepository :Repository<Company>, ICompanyRepository
    {
        private readonly DataContext _db;

        public CompanyRepository(DataContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Company company)
        {
           _db.Update(company);
        }
    }
}
