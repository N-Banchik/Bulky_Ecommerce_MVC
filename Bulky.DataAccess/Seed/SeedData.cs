using Bulky.DataAccess.Data;
using Bulky.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Seed
{
    public  class SeedData
    {
        private readonly DataContext _dataContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedData(DataContext dataContext,RoleManager<IdentityRole> roleManager)
        {
            _dataContext = dataContext;
            _roleManager = roleManager;
        }


        public  async Task SeedDataOnStartup()
        {
            if (!await _dataContext.Products.AnyAsync())
            {

            }
            else
            {

            }

            if (!await _roleManager.RoleExistsAsync(SD.Role_User_Cust))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Cust));
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Comp));
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee));
            }
        }

    }
}
