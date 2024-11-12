using EShopModulithCourse.Server.Shared.CQRS;
using FluentValidation;
using Modules.Basket.Basket.Dtos;
using Modules.Basket.Basket.Models;
using Modules.Basket.Data;

namespace Modules.Basket.Basket.Features.CreateBasket;

public record CreateBasketResult(Guid ShoppingCartId);

public record CreateBasketCommand(CreateShoppingCartDto ShoppingCart) : ICommand<CreateBasketResult>;

public class CreateBasketCommandValidator : AbstractValidator<CreateBasketCommand>
{
    public CreateBasketCommandValidator()
    {
        RuleFor(x => x.ShoppingCart.UserName)
            .NotEmpty().WithMessage("UserName is required.");
    }
}

public class CreateBasketCommandHandler : ICommandHandler<CreateBasketCommand, CreateBasketResult>
{
    private readonly BasketDbContext _basketDbContext;

    public CreateBasketCommandHandler(BasketDbContext basketDbContext)
    {
        _basketDbContext = basketDbContext;
    }

    public async Task<CreateBasketResult> Handle(CreateBasketCommand command, CancellationToken cancellationToken)
    {
        var newShoppingCart = ShoppingCart.Create(command.ShoppingCart.UserName);

        _basketDbContext.ShoppingCarts.Add(newShoppingCart);
        await _basketDbContext.SaveChangesAsync(cancellationToken);
        return new CreateBasketResult(newShoppingCart.Id);
    }
}