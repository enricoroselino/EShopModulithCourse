using EShopModulithCourse.Server.Shared.CQRS;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Modules.Basket.Basket.Dtos;
using Modules.Basket.Basket.Exceptions;
using Modules.Basket.Data;
using Modules.Catalog.Contract.Products.Features.GetProductById;

namespace Modules.Basket.Basket.Features.AddItemIntoBasket;

public record AddItemIntoBasketResult(Guid ShoppingCartId);

public record AddItemIntoBasketCommand(string UserName, AddItemIntoBasketRequest ItemRequest)
    : ICommand<AddItemIntoBasketResult>;

public class AddItemIntoBasketValidator : AbstractValidator<AddItemIntoBasketCommand>
{
    public AddItemIntoBasketValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("UserName is required.");
        RuleFor(x => x.ItemRequest.ProductId)
            .NotEmpty().WithMessage("Product Id is required.");
        RuleFor(x => x.ItemRequest.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
    }
}

public class AddItemIntoBasketCommandHandler : ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
{
    private readonly BasketDbContext _basketDbContext;
    private readonly ISender _mediator;

    public AddItemIntoBasketCommandHandler(BasketDbContext basketDbContext, ISender mediator)
    {
        _basketDbContext = basketDbContext;
        _mediator = mediator;
    }

    public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand command,
        CancellationToken cancellationToken)
    {
        // Add shopping cart item into shopping cart
        var shoppingCart = await _basketDbContext.ShoppingCarts
            .Include(x => x.Items)
            .Where(x => x.UserName == command.UserName)
            .SingleOrDefaultAsync(cancellationToken: cancellationToken);

        if (shoppingCart is null) throw new BasketNotFoundException(command.UserName);

        // Get latest product information and set Price and ProductName when adding item 
        var getProductCommand = new GetProductByIdQuery(command.ItemRequest.ProductId);
        var result = await _mediator.Send(getProductCommand, cancellationToken);

        shoppingCart.AddItem(
            result.Product.Id,
            command.ItemRequest.Quantity,
            command.ItemRequest.Color,
            result.Product.Price,
            result.Product.Name
        );
        
        await _basketDbContext.SaveChangesAsync(cancellationToken);
        return new AddItemIntoBasketResult(shoppingCart.Id);
    }
}