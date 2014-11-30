namespace Moviq.Interfaces.Models
{
    using System;
    using System.Collections.Generic;
    using System.Security.Principal;

    public interface ICartItem
    {
        Guid Guid { get; set; }
        Guid UserGuid { get; set; }
        string ProductUid { get; set; }
        string Title { get; set; }
        decimal Price { get; set; }
        string ThumbnailLink { get; set; }
        DateTime LastModified { get; set; }
        DateTime PurchaseDate { get; set; }
        int Quantity { get; set; }

        string _type { get; set; }
    }
}   
