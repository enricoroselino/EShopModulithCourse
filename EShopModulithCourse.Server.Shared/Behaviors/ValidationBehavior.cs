﻿using EShopModulithCourse.Server.Shared.CQRS;

namespace EShopModulithCourse.Server.Shared.Behaviors;

internal class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    // ReSharper disable once TooManyDeclarations
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any()) return await next();

        var context = new ValidationContext<TRequest>(request);
        var validatorTasks = validators
            .Select(async x => await x.ValidateAsync(context, cancellationToken));

        var validationResults = await Task.WhenAll(validatorTasks);
        var failures = validationResults
            .Where(r => r.Errors.Count > 0)
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Count > 0) throw new ValidationException(failures);
        return await next();
    }
}