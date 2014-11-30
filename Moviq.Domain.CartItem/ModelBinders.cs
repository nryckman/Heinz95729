namespace Moviq.Domain.CartItems
{
    using Grain.DataAccess.Sql;
    using Moviq.Interfaces.Factories;
    using Moviq.Interfaces.Models;
    using System;
    using System.Data;

    public static class ModelBinders
    {
        public static Func<IDataRecord, ICartItem> CartItemBinder(IFactory<ICartItem> CartItemFactory)
        {
            return r =>
            {
                var CartItem = CartItemFactory.GetInstance();
                    CartItem.Guid = r.GetValueOrDefault<Guid>("Id");
                    CartItem.UserGuid = r.GetValueOrDefault<Guid>("UserId");
                    CartItem.ProductUid = r.GetValueOrDefault<string>("ProductUid");
                    CartItem.LastModified = r.GetValueOrDefault<DateTime>("LastModified");
                    CartItem.PurchaseDate = r.GetValueOrDefault<DateTime>("PurchaseDate");
                    CartItem.Quantity = r.GetValueOrDefault<int>("Quantity");

                return CartItem;
            };
        }
    }
}
