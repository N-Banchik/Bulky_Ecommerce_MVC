using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Utility;
using BulkyWeb.Helpers;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {

            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(null, includeProps: "Category");
            return View(productList);
        }
        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(p => p.Id == productId, includeProps: "Category"),
                Count = 0,
                ProductId = productId
            };

            return View(cart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart cart)
        {

            var UserId = User.GetUserId();
            cart.UserId = UserId;
            ShoppingCart toUpdate = _unitOfWork.ShoppingCart.Get(u => u.UserId == UserId && u.ProductId == cart.ProductId);

            if (toUpdate != null)
            {
                toUpdate.Count = cart.Count;
                _unitOfWork.ShoppingCart.Update(toUpdate);
            }
            else
            {
                _unitOfWork.ShoppingCart.Add(cart);
            }
            _unitOfWork.SaveChanges();
            TempData["success"] = "Cart Updated successfully";
            int userCartCount = 0;
            userCartCount = _unitOfWork.ShoppingCart.Get(u => u.UserId == UserId).Count;
            HttpContext.Session.SetInt32(SD.SessionCart, userCartCount);

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}