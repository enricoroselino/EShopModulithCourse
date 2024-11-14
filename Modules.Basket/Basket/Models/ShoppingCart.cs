using Shared.Contracts.DDD;
using Shared.Providers;

namespace Modules.Basket.Basket.Models;

public class ShoppingCart : Aggregate<Guid>
{
    private ShoppingCart()
    {
    }

    public string UserName { get; private set; }
    private readonly List<ShoppingCartItem> _items = new List<ShoppingCartItem>();
    public IReadOnlyList<ShoppingCartItem> Items => _items.AsReadOnly();
    public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);

    public static ShoppingCart Create(string userName)
    {
        ArgumentException.ThrowIfNullOrEmpty(userName);

        var shoppingCart = new ShoppingCart
        {
            Id = UuidProvider.NewSequential(),
            UserName = userName,
        };

        return shoppingCart;
    }

    public void AddItem(Guid productId, int quantity, string color, decimal price, string productName)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

        var existingItem = _items.FirstOrDefault(x => x.ProductId == productId);

        if (existingItem is not null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            var newItem = ShoppingCartItem.Create(Id, productId, quantity, color, price, productName);
            _items.Add(newItem);
        }
    }

    public void RemoveItem(Guid productId)
    {
        var existingItem = _items.FirstOrDefault(x => x.ProductId == productId);
        if (existingItem is not null) _items.Remove(existingItem);
    }
}