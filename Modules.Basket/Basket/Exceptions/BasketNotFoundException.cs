﻿using Shared.Exceptions;

namespace Modules.Basket.Basket.Exceptions;

public class BasketNotFoundException : NotFoundException
{
    public BasketNotFoundException(string message) : base(message)
    {
    }

    public BasketNotFoundException(string name, object key) : base(name, key)
    {
    }
}