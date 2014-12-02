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

    public class CartModule : NancyModule
    {
        public CartModule(ICartItemDomain cartItems, IProductDomain products, IModuleHelpers helper)
        {
            this.Get["/api/cart/new"] = args =>
            {
                System.Diagnostics.Debug.WriteLine("inside new api");
                Guid guid = Guid.NewGuid();
                return "{\"cart_id\":\"" + guid.ToString() + "\"}";
            };

            this.Post["/api/cart/update"] = args =>
            {
                System.Diagnostics.Debug.WriteLine("inside update module");

                return "";
            };

            this.Post["/api/cart/add/{cart_id}/{product_id}"] = args =>
            {

                // find product
                IProduct product = products.Repo.Get(args.product_id);

                // if the item isn't in the cart and the product is not null, then add it
                if (product != null)
                {
                    // create cart item
                    ICartItem CartItem1 = new CartItem();
                    CartItem1.Guid = Guid.NewGuid();
                    CartItem1.UserGuid = Guid.NewGuid();
                    CartItem1.ProductUid = product.Uid;
                    CartItem1.Title = product.Title;
                    CartItem1.Price = product.Price;
                    CartItem1.ThumbnailLink = product.ThumbnailLink;
                    CartItem1.LastModified = new DateTime();
                    CartItem1.PurchaseDate = new DateTime();
                    CartItem1.Quantity = 1;
                }

                string message = "inside add module " + args.product_id + " and " + args.cart_id;
                System.Diagnostics.Debug.WriteLine(message);

                return "";
            };

            this.Post["/api/cart/remove/{cart_id}/{product_id}"] = args =>
            {
                string message = "inside remove module " + args.product_id + " and " + args.cart_id;
                System.Diagnostics.Debug.WriteLine(message);

                return "";
            };

            this.Get["/api/cart/list"] = args => {
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