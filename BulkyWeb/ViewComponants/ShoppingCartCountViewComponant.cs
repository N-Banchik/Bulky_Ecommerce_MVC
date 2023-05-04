using Bulky.DataAccess.Repository.IRepository;
using Bulky.Utility;
using BulkyWeb.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System.Security.Claims;

namespace BulkyWeb.ViewComponants
{
    public class ShoppingCartCountViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartCountViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ClaimsIdentity? claims = (ClaimsIdentity)User.Identity;
            string? UserId = claims?.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (claims.Claims.Any())
            {
                if (HttpContext.Session.GetInt32(SD.SessionCart) == null)
                {
                    HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(u => u.UserId == UserId).Count());

                }
                return View(HttpContext.Session.GetInt32(SD.SessionCart));
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);

            }

        }
    }
}
