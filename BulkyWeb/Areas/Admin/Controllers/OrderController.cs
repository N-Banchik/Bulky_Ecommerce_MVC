using AutoMapper;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using BulkyWeb.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM orderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            orderVM = new OrderVM()
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProps: "User"),
                OrderDetails = _unitOfWork.OrderDetails.GetAll(u => u.OrderHeaderId == id, includeProps: "Product")
            };


            return View(orderVM);
        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult UpdateDetails()
        {
            OrderHeader orderVMToDb = _unitOfWork.OrderHeader.Get(u => u.Id == orderVM.OrderHeader.Id, includeProps: "User");
            MapOrderHeader(orderVM.OrderHeader, orderVMToDb);


            _unitOfWork.OrderHeader.Update(orderVMToDb);
            _unitOfWork.SaveChanges();
            TempData["Success"] = "Order Details updated Successfully";

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult StartProcessing()
        {
            _unitOfWork.OrderHeader.UpdateStatus(orderVM.OrderHeader.Id, SD.StatusInProcess);
            _unitOfWork.SaveChanges();
            TempData["Success"] = $"Order no' - {orderVM.OrderHeader.Id} now in process";
            return RedirectToAction(nameof(Details), new { id = orderVM.OrderHeader.Id });

        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult ShipOrder()
        {
            OrderHeader orderVMToDb = _unitOfWork.OrderHeader.Get(u => u.Id == orderVM.OrderHeader.Id);
            MapOrderHeader(orderVM.OrderHeader, orderVMToDb);
            if (orderVMToDb.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                orderVMToDb.PaymentDueDate = DateTime.Now.AddDays(30);
            }
            orderVMToDb.OrderStatus = SD.StatusShipped;
            _unitOfWork.SaveChanges();
            TempData["Success"] = $"Order no' - {orderVM.OrderHeader.Id} now Shipped!";
            return RedirectToAction(nameof(Details), new { id = orderVM.OrderHeader.Id });

        }

        [HttpPost]
       
        public async Task<IActionResult> Company_PayAsync()
        {

            orderVM.OrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderVM.OrderHeader.Id, includeProps: "User");
            orderVM.OrderDetails = _unitOfWork.OrderDetails.GetAll(u => u.OrderHeaderId == orderVM.OrderHeader.Id, includeProps: "Product");

            List<SessionLineItemOptions> optionsList = new List<SessionLineItemOptions>();
            foreach (var item in orderVM.OrderDetails)
            {
                SessionLineItemOptions sessionLineItem = new SessionLineItemOptions()
                {
                    PriceData = new()
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "usd",
                        ProductData = new()
                        { Name = item.Product!.Title }
                    },
                    Quantity = item.Count
                };
                optionsList.Add(sessionLineItem);
            }
            string sucseesurl = $"admin/order/OrderConformation?orderHeadrId={orderVM.OrderHeader.Id}";
            string returnurl = $"admin/order/details?id={orderVM.OrderHeader.Id}";

            var session = await StripeHelper.PaymentOrderAsync(orderVM.OrderHeader.Id, optionsList, sucseesurl,returnurl);
            _unitOfWork.OrderHeader.UpdateStripePaymentId(orderVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.SaveChanges();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);


           

        }

        public IActionResult OrderConformation(int orderHeadrId)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(o => o.Id == orderHeadrId);
            if (orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentId(orderHeadrId, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(orderHeadrId, orderHeader.OrderStatus, SD.PaymentStatusApproved);
                    _unitOfWork.SaveChanges();
                }
            }

           

            return View(orderHeadrId);
        }

        public IActionResult CancelOrder()
        {

            OrderHeader orderVMToCancel = _unitOfWork.OrderHeader.Get(u => u.Id == orderVM.OrderHeader.Id);
            if (orderVMToCancel.PaymentStatus == SD.PaymentStatusApproved)
            {
                StripeHelper.RefundOrder(orderVMToCancel.PaymentIntentId!);
                _unitOfWork.OrderHeader.UpdateStatus(orderVMToCancel.Id, SD.StatusCanceled, SD.StatusRefunded);
            }
            else
            {
                _unitOfWork.OrderHeader.UpdateStatus(orderVMToCancel.Id, SD.StatusCanceled, SD.StatusCanceled);
            }

            _unitOfWork.SaveChanges();
            TempData["Success"] = $"Order no' - {orderVM.OrderHeader.Id} is canceled!";
            return RedirectToAction(nameof(Details), new { id = orderVM.OrderHeader.Id });

        }

        void MapOrderHeader(OrderHeader oldOrder, OrderHeader newOrder)
        {
            newOrder.Name = oldOrder.Name;
            newOrder.PhoneNumber = oldOrder.PhoneNumber;
            newOrder.StreetAddress = oldOrder.StreetAddress;
            newOrder.City = oldOrder.City;
            newOrder.Country = oldOrder.Country;
            newOrder.Zipcode = oldOrder.Zipcode;
            if (!string.IsNullOrEmpty(oldOrder.Carrier))
            {
                newOrder.Carrier = oldOrder.Carrier;
            }
            if (!string.IsNullOrEmpty(oldOrder.TrackingNumber))
            {
                newOrder.TrackingNumber = oldOrder.TrackingNumber;
            }
        }
        #region API Calls
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaders = new List<OrderHeader>();
            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                orderHeaders = (List<OrderHeader>)_unitOfWork.OrderHeader.GetAll(includeProps: "User");
            }
            else
            {
                string? UserId = User.GetUserId();
                orderHeaders = (List<OrderHeader>)_unitOfWork.OrderHeader.GetAll(u => u.UserId == UserId, includeProps: "User");

            }

            switch (status)
            {
                case "inprocess":

                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "pending":
                    orderHeaders = orderHeaders.Where(u => u.PaymentStatus == SD.PaymentStatusPending);

                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);

                    break;
                case "approved":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);

                    break;
                default:
                    break;

            }
            return Json(new { data = orderHeaders });
        }
        #endregion
    }
}
