using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Modules.Basket.Data;
using Shared.Contracts.CQRS;

namespace Modules.Basket.Basket.Features.UpdateItemPriceInBasket;

public record UpdateItemPriceInBasketResult(bool IsSuccess);

public record UpdateItemPriceInBasketCommand(Guid ProductId, decimal Price) : ICommand<UpdateItemPriceInBasketResult>;

public class UpdateItemPriceInBasketCommandValidator : AbstractValidator<UpdateItemPriceInBasketCommand>
{
    public UpdateItemPriceInBasketCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");
    }
}

internal class
    UpdateItemPriceInBasketCommandHandler :
    ICommandHandler<UpdateItemPriceInBasketCommand, UpdateItemPriceInBasketResult>
{
    private readonly BasketDbContext _basketDbContext;

    public UpdateItemPriceInBasketCommandHandler(BasketDbContext basketDbContext)
    {
        _basketDbContext = basketDbContext;
    }

    public async Task<UpdateItemPriceInBasketResult> Handle(UpdateItemPriceInBasketCommand request,
        CancellationToken cancellationToken)
    {
        // avoid n + 1 problem for using for loop
        await using var transaction = await _basketDbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await _basketDbContext.ShoppingCartItems
                .Where(x => x.ProductId == request.ProductId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.Price, request.Price), cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            return new UpdateItemPriceInBasketResult(true);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            return new UpdateItemPriceInBasketResult(false);
        }
    }
}