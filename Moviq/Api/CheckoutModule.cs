namespace Moviq.Api
{
    using Moviq.Domain.CartItem;
    using Moviq.Domain.Products;
    using Moviq.Helpers;
    using Moviq.Interfaces.Models;
    using Moviq.Interfaces.Services;
    using Nancy;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xamarin.Payments.Stripe;

    public class CheckoutModule : NancyModule
    {
        public CheckoutModule(ICartItemDomain cartItems, IProductDomain products, IModuleHelpers helper)
        {
            this.Post["/api/checkout/pay/{cart_id}/{card_number}/{cvc}/{exp_month}/{exp_year}"] = args =>
            {
                // build credit card model
                StripeCreditCardInfo cc = new StripeCreditCardInfo();
                cc.Number = args.card_number;
                cc.CVC = args.cvc;
                cc.ExpirationMonth = args.exp_month;
                cc.ExpirationYear = args.exp_year;

                // calculate total cost of items
                List<ICartItem> cartList = cartItems.Repo.GetCartItems(args.cart_id);
                int cartCount = cartList==null?0:cartList.Count;
                int totalCost = 0;
                for (int i = 0; i < cartCount; i++)
                {
                    totalCost += Convert.ToInt32(cartList[i].Price * 100) * cartList[i].Quantity;
                }

                // charge credit card
                if (totalCost > 0)
                {
                    System.Diagnostics.Debug.WriteLine("cost: " + totalCost);
                    StripePayment payment = new StripePayment("sk_test_S9tiUYvwqfDyIhFWS7VReNzF");
                    StripeCharge charge = payment.Charge(totalCost, "usd", cc, "Test charge");
                    System.Diagnostics.Debug.WriteLine(charge);
                }

                // mark cart items as purchased using datetime stamp
                for (int i = 0; i < cartCount; i++)
                {
                    cartList[i].PurchaseDate = DateTime.Now;
                    cartItems.Repo.Set(cartList[i]);
                }

                return "";
            };
        }
    }
}