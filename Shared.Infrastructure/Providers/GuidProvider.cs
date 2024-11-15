using Shared.Infrastructure.Providers.Interfaces;
using UUIDNext;

namespace Shared.Infrastructure.Providers;

public class GuidProvider : IGuidProvider
{
    public Guid NewRandom() => Guid.NewGuid();
    public Guid NewSequential() => Uuid.NewSequential();
}