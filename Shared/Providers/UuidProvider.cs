using UUIDNext;

namespace Shared.Providers;

public interface IUuidProvider
{
    public Guid NewRandom();
    public Guid NewSequential();
}

public class UuidProvider : IUuidProvider
{
    public Guid NewRandom()
    {
        return Guid.NewGuid();
    }

    public Guid NewSequential()
    {
        return Uuid.NewSequential();
    }
}