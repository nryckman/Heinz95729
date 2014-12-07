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

    public class CartModule : NancyModule
    {
        public static ArrayList CartItemList = new ArrayList();

        public CartModule(ICartItemDomain cartItems, IProductDomain products, IModuleHelpers helper)
        {
            this.Get["/api/cart/{cart_id}/count"] = args =>
            {
                System.Diagnostics.Debug.WriteLine("inside cart count api");
                List<ICartItem> cartList = cartItems.Repo.GetCartItems(args.cart_id);
                int cartCount = cartList == null ? 0 : cartList.Count;
                return "{\"cart_count\":" + cartCount + "}";
            };

            this.Get["/api/cart/new"] = args =>
            {
                System.Diagnostics.Debug.WriteLine("inside new api");
                Guid guid = Guid.NewGuid();
                return "{\"cart_id\":\"" + guid.ToString() + "\"}";
            };

            this.Post["/api/cart/update/{cart_id}/{product_id}/quantity/{quantity}"] = args =>
            {
                System.Diagnostics.Debug.WriteLine("inside update module");

                // get composite key
                string compositeKeyPattern = "{0}::{1}";
                string compositeKey = String.Format(compositeKeyPattern, args.cart_id, args.product_id);
                
                ICartItem cartItem = cartItems.Repo.Get(compositeKey);
                if (cartItem != null)
                {
                    cartItem.Quantity = args.quantity;
                    cartItems.Repo.Set(cartItem);
                }

                return "";
            };

            this.Post["/api/cart/add/{cart_id}/{product_id}"] = args =>
            {
                // get composite key
                string compositeKeyPattern = "{0}::{1}";
                string compositeKey = String.Format(compositeKeyPattern, args.cart_id, args.product_id);

                // find cart item (need to bind...)
                ICartItem cartItem = cartItems.Repo.Get(compositeKey);


                string message = "inside add module " + args.cart_id + " and " + args.product_id;
                System.Diagnostics.Debug.WriteLine(message);


                // if the cart item can't be found, add a new one
                if (cartItem == null)
                {
                    // find product info.
                    IProduct product = products.Repo.Get(args.product_id);

                    // create cart item from product info.
                    cartItem = new CartItem();
                    cartItem.Guid = args.cart_id;
                    cartItem.UserGuid = Guid.NewGuid();
                    cartItem.ProductUid = product.Uid;
                    cartItem.Title = product.Title;
                    cartItem.Price = product.Price;
                    cartItem.ThumbnailLink = product.ThumbnailLink;
                    cartItem.LastModified = new DateTime();
                    cartItem.PurchaseDate = new DateTime();
                    cartItem.Quantity = 1;

                    // add cart Item
                    cartItem = cartItems.Repo.Set(cartItem);
                }

                message = cartItem == null ? "didn't work" : "worked: " + cartItem.ProductUid;
                System.Diagnostics.Debug.WriteLine(message);

                
                return "";
            };

            this.Post["/api/cart/remove/{cart_id}/{product_id}"] = args =>
            {
                string message = "inside remove module " + args.product_id + " and " + args.cart_id;
                System.Diagnostics.Debug.WriteLine(message);

                // get composite key
                string compositeKeyPattern = "{0}::{1}";
                string compositeKey = String.Format(compositeKeyPattern, args.cart_id, args.product_id);

                return cartItems.Repo.Delete(compositeKey);
            };

            this.Get["/api/cart/list/{cart_id}"] = args => {
                List<ICartItem> cartList  = cartItems.Repo.GetCartItems(args.cart_id);
                System.Diagnostics.Debug.WriteLine("inside list module" + cartList.Count);

                return helper.ToJson(cartList);
            };
        }
    }
}




//        int index = -1;
 //               for (int i = 0; i < CartItemList.Count; i++)
  //              {
    //                ICartItem cartItem = (ICartItem)CartItemList[i];
   //                 if (cartItem.ProductUid.ToString().Equals(args.product_id))
    //                {
     //                   index = i;
    //                    break;
      //              }
       //         }

       //         if (index > -1)
        //        {
         //           CartItemList.RemoveAt(index);
          //      }
//return helper.ToJson(cartList);


// create mock cart items
//ICartItem CartItem1 = new CartItem();
//CartItem1.Guid = Guid.NewGuid();
//CartItem1.UserGuid = Guid.NewGuid();
//CartItem1.ProductUid = "dirk_gentlys_detective_agency";
//CartItem1.Title = "Dirk Gently's Holistic Detective Agency";
//CartItem1.Price = 6.83m;
//CartItem1.ThumbnailLink = "/images/books/dirkgently.jpeg";
//CartItem1.LastModified = new DateTime();
//CartItem1.PurchaseDate = new DateTime();
//CartItem1.Quantity = 2;

//ICartItem CartItem2 = new CartItem();
//CartItem2.Guid = Guid.NewGuid();
//CartItem2.UserGuid = Guid.NewGuid();
//CartItem2.ProductUid = "universe_everything";
//CartItem2.Title = "Life, the Universe and Everything";
//CartItem2.Price = 5.99m;
//CartItem2.ThumbnailLink = "/images/books/lifeandeverything.jpeg";
//CartItem2.LastModified = new DateTime();
//CartItem2.PurchaseDate = new DateTime();
//CartItem2.Quantity = 3;

//ICartItem[] CartItemList = new ICartItem[2]
//{
//    CartItem1,
//    CartItem2
//};
//cartItems.Repo.Get(CartItem1);
// return them as JSON


//IEnumerable<ICartItem> cartList = cartItems.Repo.GetCartItems("55002807-d243-430a-9d62-cd5de5fd6ac8");
//foreach(ICartItem cartItem in cartList){
//    System.Diagnostics.Debug.WriteLine(cartItem.Guid);
//    System.Diagnostics.Debug.WriteLine(helper.ToJson(cartItem));
//}

