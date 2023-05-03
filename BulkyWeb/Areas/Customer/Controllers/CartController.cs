using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using BulkyWeb.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace BulkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartVM? ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            ShoppingCartVM = new ShoppingCartVM();
        }
        public IActionResult Index()
        {
            string? UserId = User.GetUserId();
            ShoppingCartVM cartVM = new ShoppingCartVM();
            cartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.UserId == UserId, includeProps: "Product");
            HttpContext.Session.SetInt32(SD.SessionCart, cartVM.ShoppingCartList.Count());
            cartVM.OrderHeader = new OrderHeader();
            foreach (var item in cartVM.ShoppingCartList)
            {
                item.PriceTotal = GetPriceTotal(item);
                cartVM.OrderHeader.OrderTotal += item.PriceTotal * item.Count;
                cartVM.OrderHeader.OrderTotal = Math.Round(cartVM.OrderHeader.OrderTotal, 2);

            }
            return View(cartVM);
        }

        public IActionResult Summery()
        {

            string? UserId = User.GetUserId();
            ShoppingCartVM cartVM = new ShoppingCartVM();
            cartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.UserId == UserId, includeProps: "Product");
            cartVM.OrderHeader = new OrderHeader();
            cartVM.OrderHeader.User = _unitOfWork.AppUSer.Get(i => i.Id == UserId);
            MapOrderHeader(cartVM.OrderHeader.User, cartVM.OrderHeader);
            foreach (var item in cartVM.ShoppingCartList)
            {
                item.PriceTotal = GetPriceTotal(item);
                cartVM.OrderHeader.OrderTotal += item.PriceTotal * item.Count;
                cartVM.OrderHeader.OrderTotal = Math.Round(cartVM.OrderHeader.OrderTotal, 2);

            }
            return View(cartVM);
        }
        [HttpPost]
        [ActionName("Summery")]
        public async Task<IActionResult> SummeryPost()
        {

            string? UserId = User.GetUserId();
            ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.UserId == UserId, includeProps: "Product");
            ShoppingCartVM.OrderHeader = new OrderHeader();
            ShoppingCartVM.OrderHeader.User = _unitOfWork.AppUSer.Get(i => i.Id == UserId);

            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.UserId = UserId;
            MapOrderHeader(ShoppingCartVM.OrderHeader.User, ShoppingCartVM.OrderHeader);

            foreach (var item in ShoppingCartVM.ShoppingCartList)
            {
                item.PriceTotal = GetPriceTotal(item);
                ShoppingCartVM.OrderHeader.OrderTotal += item.PriceTotal * item.Count;
                ShoppingCartVM.OrderHeader.OrderTotal = Math.Round(ShoppingCartVM.OrderHeader.OrderTotal, 2);

            }

            if (ShoppingCartVM.OrderHeader.User.CompanyId.GetValueOrDefault() == 0)
            {
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            }
            else
            {
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;

            }

            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.SaveChanges();

            foreach (var item in ShoppingCartVM.ShoppingCartList)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = item.ProductId,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                    Price = item.PriceTotal,
                    Count = item.Count,
                };
                _unitOfWork.OrderDetails.Add(orderDetail);
            }
            _unitOfWork.SaveChanges();

            if (ShoppingCartVM.OrderHeader.User.CompanyId.GetValueOrDefault() == 0)
            {

                List<SessionLineItemOptions> optionsList = new List<SessionLineItemOptions>();
                foreach (var item in ShoppingCartVM.ShoppingCartList)
                {
                    SessionLineItemOptions sessionLineItem = new SessionLineItemOptions()
                    {
                        PriceData = new()
                        {
                            UnitAmount = (long)(item.PriceTotal * 100),
                            Currency = "usd",
                            ProductData = new()
                            { Name = item.Product!.Title }
                        },
                        Quantity = item.Count
                    };
                    optionsList.Add(sessionLineItem);
                }
                var session = await StripeHelper.PaymentOrderAsync(ShoppingCartVM.OrderHeader.Id, optionsList);
                _unitOfWork.OrderHeader.UpdateStripePaymentId(ShoppingCartVM.OrderHeader.Id, session.Id, string.Empty);
                _unitOfWork.SaveChanges();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }
            return RedirectToAction(nameof(OrderConformation), new { id = ShoppingCartVM.OrderHeader.Id });
        }

        public IActionResult OrderConformation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(o => o.Id == id);
            if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentId(id, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                }
            }

            List<ShoppingCart> shoppingCarts =
                _unitOfWork.ShoppingCart.GetAll(s => s.UserId == orderHeader.UserId).ToList();

            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.SaveChanges();

            return View(id);
        }

        public IActionResult AddQuantity(int cartId)
        {
            ShoppingCart cart = _unitOfWork.ShoppingCart.Get(x => x.Id == cartId);
            cart.Count++;
            _unitOfWork.ShoppingCart.Update(cart);
            _unitOfWork.SaveChanges();
            TempData["success"] = "Cart Updated successfully";

            return RedirectToAction(nameof(Index));
        }
        public IActionResult SubtractQuantity(int cartId)
        {
            ShoppingCart cart = _unitOfWork.ShoppingCart.Get(x => x.Id == cartId);
            cart.Count--;
            if (cart.Count == 0)
            {
                Remove(cart.Id, cart);
                return RedirectToAction(nameof(Index));

            }
            _unitOfWork.ShoppingCart.Update(cart);
            _unitOfWork.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId, ShoppingCart cart = null)
        {
            if (cart.Id != 0)
            {
                _unitOfWork.ShoppingCart.Remove(cart);
                _unitOfWork.SaveChanges();
                return RedirectToAction(nameof(Index));

            }
            ShoppingCart cartTorRmove = _unitOfWork.ShoppingCart.Get(x => x.Id == cartId);

            _unitOfWork.ShoppingCart.Remove(cartTorRmove);
            _unitOfWork.SaveChanges();
            TempData["success"] = "Cart Updated successfully";

            return RedirectToAction(nameof(Index));

        }


        private double GetPriceTotal(ShoppingCart cart)
        {

            if (cart.Count < 50)
            {
                return cart!.Product!.Price;
            }
            else if (cart.Count >= 50 && cart.Count < 100)
            {
                return cart!.Product!.Price50;
            }
            return cart!.Product!.Price100;


        }
        void MapOrderHeader(AppUser User, OrderHeader newOrder)
        {
            newOrder.Name = User.Name;
            newOrder.PhoneNumber = User.PhoneNumber;
            newOrder.StreetAddress = User.StreetAddress;
            newOrder.City = User.City;
            newOrder.Country = User.Country;
            newOrder.Zipcode = User.Zipcode;

        }
    }
}
