namespace EShopModulithCourse.Server.Shared.DDD;

public interface IEntity : IAuditableEntity
{
}

public interface IEntity<TKey> : IEntity
{
    public TKey Id { get; set; }
}

public abstract class Entity<TKey> : IEntity<TKey>
{
    public required TKey Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}