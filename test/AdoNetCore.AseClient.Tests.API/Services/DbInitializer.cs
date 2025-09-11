using Microsoft.EntityFrameworkCore;

public static class DbInitializer
{
    public static async Task SeedAsync(AseDbContext context)
    {
        // await context.Database.EnsureDeletedAsync();
        // await context.Database.EnsureCreatedAsync();
        // await AddDummyEntities(context);
        await context.Database.ExecuteSqlRawAsync(@"
-- Drop the table if it exists
IF EXISTS (SELECT 1 FROM sysobjects WHERE name = 'DummyEntities' AND type = 'U')
BEGIN
    DROP TABLE DummyEntities
END
");

        await context.Database.ExecuteSqlRawAsync(@"

-- Create the table with Sybase-style identity column
CREATE TABLE DummyEntities (
    Id NUMERIC(10,0) IDENTITY PRIMARY KEY,
    Name VARCHAR(255) NOT NULL
)
        ");
    }

    private static async Task AddDummyEntities(AseDbContext context)
    {
        if (!context.DummyEntities.Any())
        {
            context.DummyEntities.AddRange(new[]
            {
                new DummyEntity { Name = "Seed Record 1" },
                new DummyEntity { Name = "Seed Record 2" },
                new DummyEntity { Name = "Seed Record 3" }
            });

            await context.SaveChangesAsync();
        }
    }
}