namespace Shared.Infrastructure.Providers.Interfaces;

public interface IGuidProvider
{
    public Guid NewRandom();
    public Guid NewSequential();
}