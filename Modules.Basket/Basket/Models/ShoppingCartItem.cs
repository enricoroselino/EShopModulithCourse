﻿using EShopModulithCourse.Server.Shared.Providers;

namespace Modules.Basket.Basket.Models;

public class ShoppingCartItem : Entity<Guid>
{
    private ShoppingCartItem()
    {
    }

    public Guid ShoppingCartId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; internal set; }
    public string Color { get; private set; } = default!;


    // will comes from Catalog module
    public decimal Price { get; private set; }
    public string ProductName { get; private set; } = default!;

    internal ShoppingCartItem(Guid shoppingCartId, Guid productId, int quantity, string color, decimal price,
        string productName)
    {
        ShoppingCartId = shoppingCartId;
        ProductId = productId;
        Quantity = quantity;
        Color = color;
        Price = price;
        ProductName = productName;
    }

    public void UpdatePrice(decimal newPrice)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(newPrice);
        Price = newPrice;
    }
}