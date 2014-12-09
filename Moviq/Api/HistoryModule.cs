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
    using System.Collections.Generic;

    public class HistoryModule : NancyModule
    {
        public HistoryModule(ICartItemDomain cartItems, IProductDomain products, IModuleHelpers helper)
        {
           

            this.Get["/api/history/list"] = args => {
                System.Diagnostics.Debug.WriteLine("inside history list module");
                List<ICartItem> cartList = cartItems.Repo.GetPurchasedCartItems();
                return helper.ToJson(cartList);
            };
        }
    }
}