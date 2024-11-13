﻿namespace Modules.Basket.Basket.Dtos;

public record ShoppingCartItemDto(
    Guid ShoppingCartId,
    Guid ProductId,
    int Quantity,
    string Color,
    decimal Price,
    string ProductName
);