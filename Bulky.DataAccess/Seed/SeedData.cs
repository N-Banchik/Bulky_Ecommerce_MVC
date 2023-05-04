using Bulky.DataAccess.Data;
using Bulky.Models.Models;
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
        private readonly UserManager<IdentityUser> _userManager;

        public SeedData(DataContext dataContext,RoleManager<IdentityRole> roleManager,UserManager<IdentityUser> userManager)
        {
            _dataContext = dataContext;
            _roleManager = roleManager;
            _userManager = userManager;
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
                var user = new AppUser()
                {
                    Name="johnny boy",
                    Email="Admin@gmail.com",
                    PhoneNumber="0544444123",
                };
               
                await _userManager.CreateAsync(user,"Password123!");
                await _userManager.AddToRoleAsync(user,SD.Role_Admin);


            }
        }

    }
}
