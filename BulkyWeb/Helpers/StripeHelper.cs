using Microsoft.IdentityModel.Tokens;
using Stripe;
using Stripe.Checkout;

namespace BulkyWeb.Helpers
{
    public static class StripeHelper
    {
        public  static async Task<Session> PaymentOrderAsync(int id, List<SessionLineItemOptions> itemList,string? sucssesUrl=null,string? returnUrl=null)
        {
            var domain = "https://localhost:7001/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = sucssesUrl.IsNullOrEmpty()?domain + $"customer/cart/OrderConformation?id={id}":domain+ sucssesUrl,
                CancelUrl =returnUrl.IsNullOrEmpty()? domain + "customer/cart/index":domain+returnUrl,
                LineItems = itemList,
                Mode = "payment",
            };
            var service = new SessionService();
            Session session = await service.CreateAsync(options);
            return session;
        }
        public async static void RefundOrder(string paymentIntentId)
        {
            try
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = paymentIntentId
                };

                var service = new RefundService();
                Refund refund = await service.CreateAsync(options);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
