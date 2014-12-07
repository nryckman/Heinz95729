namespace Moviq.Domain.CartItem
{
    using Moviq.Interfaces.Models;
    using Moviq.Interfaces.Repositories;
    using Moviq.Interfaces.Services;

    public class CartItemDomain : ICartItemDomain
    {
        public CartItemDomain(ICartItemRepository<ICartItem> repo)
        {
            this.Repo = repo;
        }

        public ICartItemRepository<ICartItem> Repo { get; set; }
    }
}
