using Moviq.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moviq.Domain.CartItems
{
    public class CartItem : ICartItem, IHelpCategorizeNoSqlData
    {
        public CartItem() 
        {
            this._type = "cart_item";
        }

        public Guid Guid { get; set; }
        public Guid UserGuid { get; set; }
        public string ProductUid { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string ThumbnailLink { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int Quantity { get; set; }
        public string _type { get; set; }
    }
}
