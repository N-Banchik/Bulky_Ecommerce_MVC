using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class AppUserRepository : Repository<AppUser>, IAppUSerRepository
    {
        private readonly DataContext _db;

        public AppUserRepository(DataContext db) : base(db)
        {
            _db = db;
        }

        public void Update(AppUser appUser)
        {
            _db.Users.Update(appUser);
        }
    }
}
