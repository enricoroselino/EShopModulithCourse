﻿namespace EShopModulithCourse.Server.Shared.CQRS;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}

public interface ICommand : ICommand<Unit>
{
}