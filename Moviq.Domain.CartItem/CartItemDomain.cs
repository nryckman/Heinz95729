namespace Moviq.Domain.CartItems
{
    using Moviq.Interfaces.Models;
    using Moviq.Interfaces.Repositories;
    using Moviq.Interfaces.Services;

    public class CartItemDomain : ICartItemDomain
    {
        public CartItemDomain(IRepository<ICartItem> repo)
        {
            this.Repo = repo;
        }

        public IRepository<ICartItem> Repo { get; set; }
    }
}
