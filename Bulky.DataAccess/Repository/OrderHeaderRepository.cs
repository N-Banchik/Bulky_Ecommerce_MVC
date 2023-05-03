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
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly DataContext _db;

        public OrderHeaderRepository(DataContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader orderHeader)
        {
            _db.OrderHeaders.Update(orderHeader);
        }

        public void UpdateStatus(int id, string OrderStatus, string? paymentStatus = null)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(i => i.Id == id);
            if (orderFromDb != null) 
            {
                orderFromDb.OrderStatus = OrderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }

        public void UpdateStripePaymentId(int id, string sessionId, string? paymentItentId)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(i => i.Id == id);
            if (!string.IsNullOrEmpty(sessionId))
            {
                orderFromDb.SessionId= sessionId;
            }

            if (!string.IsNullOrEmpty(paymentItentId))
            {
                orderFromDb.PaymentIntentId = paymentItentId;
                orderFromDb.PaymentDate = DateTime.Now;
            }
        }
    }
}
