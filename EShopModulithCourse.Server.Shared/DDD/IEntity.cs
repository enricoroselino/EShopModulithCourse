namespace EShopModulithCourse.Server.Shared.DDD;

public interface IEntity
{
    public DateTime? CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}

public interface IEntity<TKey> : IEntity
{
    public TKey Id { get; set; }
}