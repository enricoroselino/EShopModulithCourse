using EShopModulithCourse.Server.Shared.CQRS;
using FluentValidation;
using Modules.Catalog.Data;
using Modules.Catalog.Products.Dtos;
using Modules.Catalog.Products.Models;

namespace Modules.Catalog.Products.Features.CreateProduct;

public record CreateProductResult(Guid Id);

public record CreateProductCommand(CreateProductDto Product) : ICommand<CreateProductResult>;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Product.Name)
            .NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Product.Categories)
            .NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.Product.ImageUrl)
            .NotEmpty().WithMessage("Image url is required");
        RuleFor(x => x.Product.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    private readonly CatalogDbContext _catalogDbContext;

    public CreateProductCommandHandler(CatalogDbContext catalogDbContext)
    {
        _catalogDbContext = catalogDbContext;
    }
    
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var newProduct = Product.Create(
            command.Product.Name,
            command.Product.Description,
            command.Product.Categories,
            command.Product.Price,
            command.Product.ImageUrl
        );

        await _catalogDbContext.Products.AddAsync(newProduct, cancellationToken);
        await _catalogDbContext.SaveChangesAsync(cancellationToken);
        return new CreateProductResult(newProduct.Id);
    }
}