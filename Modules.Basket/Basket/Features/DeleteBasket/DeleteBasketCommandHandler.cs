using EShopModulithCourse.Server.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Modules.Basket.Basket.Exceptions;
using Modules.Basket.Data;

namespace Modules.Basket.Basket.Features.DeleteBasket;

public record DeleteBasketCommandResult(bool IsSuccess);

public record DeleteBasketCommand(string UserName) : ICommand<DeleteBasketCommandResult>;

public class DeleteBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
{
    public DeleteBasketCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
    }
}

public class DeleteBasketCommandHandler : ICommandHandler<DeleteBasketCommand, DeleteBasketCommandResult>
{
    private readonly BasketDbContext _basketDbContext;


    public DeleteBasketCommandHandler(BasketDbContext basketDbContext)
    {
        _basketDbContext = basketDbContext;
    }

    public async Task<DeleteBasketCommandResult> Handle(DeleteBasketCommand command,
        CancellationToken cancellationToken)
    {
        var basket = await _basketDbContext.ShoppingCarts
            .SingleOrDefaultAsync(x => x.UserName == command.UserName, cancellationToken);

        if (basket is null) throw new BasketNotFoundException(command.UserName);

        _basketDbContext.ShoppingCarts.Remove(basket);
        await _basketDbContext.SaveChangesAsync(cancellationToken);
        return new DeleteBasketCommandResult(true);
    }
}