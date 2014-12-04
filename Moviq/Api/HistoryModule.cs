namespace Moviq.Api
{
    using Moviq.Domain.CartItems;
    using Moviq.Domain.Products;
    using Moviq.Helpers;
    using Moviq.Interfaces.Models;
    using Moviq.Interfaces.Services;
    using Nancy;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System;

    public class HistoryModule : NancyModule
    {
        public HistoryModule(ICartItemDomain cartItems, IProductDomain products, IModuleHelpers helper)
        {
           

            this.Get["/api/history/list"] = args => {
                System.Diagnostics.Debug.WriteLine("inside checkout module");

                // create mock cart items
                ICartItem CartItem1 = new CartItem();
                CartItem1.Guid = Guid.NewGuid();
                CartItem1.UserGuid = Guid.NewGuid();
                CartItem1.ProductUid = "dirk_gentlys_detective_agency";
                CartItem1.Title = "Dirk Gently's Holistic Detective Agency";
                CartItem1.Price = 6.83m;
                CartItem1.ThumbnailLink = "/images/books/dirkgently.jpeg";
                CartItem1.LastModified = new DateTime();
                CartItem1.PurchaseDate = new DateTime();
                CartItem1.Quantity = 2;

                ICartItem CartItem2 = new CartItem();
                CartItem2.Guid = Guid.NewGuid();
                CartItem2.UserGuid = Guid.NewGuid();
                CartItem2.ProductUid = "universe_everything";
                CartItem2.Title = "Life, the Universe and Everything";
                CartItem2.Price = 5.99m;
                CartItem2.ThumbnailLink = "/images/books/lifeandeverything.jpeg";
                CartItem2.LastModified = new DateTime();
                CartItem2.PurchaseDate = new DateTime();
                CartItem2.Quantity = 3;

                ICartItem[] CartItemList = new ICartItem[2]
                {
                    CartItem1,
                    CartItem2
                };
              
                // return them as JSON
                return helper.ToJson(CartItemList);
            };
        }
    }
}