namespace Moviq.Api
{
    using Moviq.Domain.Products;
    using Moviq.Helpers;
    using Moviq.Interfaces.Models;
    using Moviq.Interfaces.Services;
    using Nancy;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class CartModule : NancyModule
    {
        public CartModule(IModuleHelpers helper)
        {
            this.Post["/api/cart"] = args =>
            {
                System.Diagnostics.Debug.WriteLine("inside add module");

                return "";
            };

            this.Delete["/api/cart"] = args =>
            {
                System.Diagnostics.Debug.WriteLine("inside delete module");

                return "";
            };

            this.Get["/api/cart"] = args => {
                System.Diagnostics.Debug.WriteLine("inside checkout module");
                
                IProduct product1 = new Product();
                product1.Uid = "dirk_gentlys_detective_agency";
                product1.Title = "Dirk Gently's Holistic Detective Agency";
                product1.Price = 6.83m;
                product1.ThumbnailLink = "/images/books/dirkgently.jpeg";

                IProduct product2 = new Product();
                product2.Uid = "universe_everything";
                product2.Title = "Life, the Universe and Everything";
                product2.Price = 5.99m;
                product2.ThumbnailLink = "/images/books/lifeandeverything.jpeg";

                IProduct[] products = new IProduct[2]
                {
                    product1,
                    product2,
                };

                return helper.ToJson(products);
            };
        }
    }
}