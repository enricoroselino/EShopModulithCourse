using Microsoft.EntityFrameworkCore;
using Modules.Basket.Basket.Dtos;
using Modules.Basket.Basket.Exceptions;
using Modules.Basket.Data;
using Shared.Contracts.CQRS;

namespace Modules.Basket.Basket.Features.GetBasket;

public record GetBasketResult(ShoppingCartDto ShoppingCart);

public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;

public class GetBasketQueryHandler : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    private readonly BasketDbContext _basketDbContext;

    public GetBasketQueryHandler(BasketDbContext basketDbContext)
    {
        _basketDbContext = basketDbContext;
    }


    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken cancellationToken)
    {
        var basket = await _basketDbContext.ShoppingCarts
            .AsNoTracking()
            .Include(cart => cart.Items)
            .AsSplitQuery()
            .Where(cart => cart.UserName == query.UserName)
            .Select(cart => new ShoppingCartDto(
                    cart.Id,
                    cart.Items.Select(item => new ShoppingCartItemDto(
                            cart.Id,
                            item.ProductId,
                            item.Quantity,
                            item.Color,
                            item.Price,
                            item.ProductName))
                        .ToList()
                )
            ).SingleOrDefaultAsync(cancellationToken);


        if (basket is null) throw new BasketNotFoundException(query.UserName);
        return new GetBasketResult(basket);
    }
}