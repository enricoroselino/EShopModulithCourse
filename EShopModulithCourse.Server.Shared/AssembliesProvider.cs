namespace EShopModulithCourse.Server.Shared;

internal static class AssembliesProvider
{
    public static Type[] GetInterfaceTypes<T>(params Assembly[] assemblies)
    {
        if (!typeof(T).IsInterface) throw new ArgumentException("T must be an interface");

        var types = assemblies
            .Where(a => !a.IsDynamic)
            .SelectMany(a => a.GetTypes())
            .Where(x => x is { IsClass: true, IsAbstract: false } && x.IsAssignableTo(typeof(T)))
            .ToArray();

        return types;
    }
}