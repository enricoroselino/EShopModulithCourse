using UUIDNext;

namespace EShopModulithCourse.Server.Shared.Providers;

public static class UuidProvider
{
    public static Guid NewRandom()
    {
        return Guid.NewGuid();
    }

    public static Guid NewSequential()
    {
        return Uuid.NewSequential();
    }
}