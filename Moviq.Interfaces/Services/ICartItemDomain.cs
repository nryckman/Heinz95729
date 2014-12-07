namespace Moviq.Interfaces.Services
{
    using Moviq.Interfaces.Models;
    using Moviq.Interfaces.Repositories;

    public interface ICartItemDomain
    {
        ICartItemRepository<ICartItem> Repo { get; set; }
    }
}
