namespace EShopModulithCourse.Server.Shared.DDD;

public interface IAuditableEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}