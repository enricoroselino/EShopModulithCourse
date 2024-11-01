namespace EShopModulithCourse.Server.Shared;

public interface IDataSeeder
{
    bool IsDummyData { get; }
    Task SeedAsync();
}