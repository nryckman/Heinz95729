namespace Moviq.Domain.CartItems
{
    using Moviq.Interfaces.Factories;
    using Moviq.Interfaces.Models;

    public class CartItemFactory : IFactory<ICartItem>
    {
        public ICartItem GetInstance()
        {
            return new CartItem() as ICartItem;
        }
    }
}
