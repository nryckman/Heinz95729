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

    public class CheckoutModule : NancyModule
    {

        public CheckoutModule(ICartItemDomain cartItems, IProductDomain products, IModuleHelpers helper)
        {
     
        }
    }
}