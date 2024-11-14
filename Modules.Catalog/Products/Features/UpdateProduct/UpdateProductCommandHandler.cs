using FluentValidation;
using Modules.Catalog.Data;
using Modules.Catalog.Products.Dtos;
using Modules.Catalog.Products.Exceptions;
using Shared.Contracts.CQRS;

namespace Modules.Catalog.Products.Features.UpdateProduct;

public record UpdateProductResult(bool IsSuccess);

public record UpdateProductCommand(UpdateProductDto Product) : ICommand<UpdateProductResult>;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Product.Id)
            .NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Product.Name)
            .NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Product.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

public record UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly CatalogDbContext _catalogDbContext;

    public UpdateProductCommandHandler(CatalogDbContext catalogDbContext)
    {
        _catalogDbContext = catalogDbContext;
    }


    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _catalogDbContext.Products
            .FindAsync([command.Product.Id], cancellationToken: cancellationToken);

        if (product is null) throw new ProductNotFoundException(command.Product.Id);

        product.Update(command.Product.Name, command.Product.Price);
        await _catalogDbContext.SaveChangesAsync(cancellationToken);
        return new UpdateProductResult(true);
    }
}