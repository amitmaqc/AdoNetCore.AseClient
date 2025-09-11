using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class DummyController : ControllerBase
{
    private readonly IDbContextFactory<AseDbContext> contextFactory;
    private readonly ILogger<DummyController> logger;

    public DummyController(IDbContextFactory<AseDbContext> contextFactory, ILogger<DummyController> logger)
    {
        this.contextFactory = contextFactory;
        this.logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] DummyEntity entity)
    {
        using var _context = contextFactory.CreateDbContext();
        _context.DummyEntities.Add(entity);
        await _context.SaveChangesAsync();
        logger.LogTrace($"Inserted entity (Id: {entity.Id}): {entity.Name}");
        return Ok(entity);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Read(int id)
    {
        logger.LogTrace($"Get entity: {id}");
        using var _context = contextFactory.CreateDbContext();
        var item = await _context.DummyEntities.FirstOrDefaultAsync(d=>d.Id == id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] DummyEntity updated)
    {
        logger.LogTrace($"Update entity: {id}");
        using var _context = contextFactory.CreateDbContext();
        var entity = await _context.DummyEntities.FindAsync(id);
        if (entity == null) return NotFound();

        entity.Name = updated.Name;
        await _context.SaveChangesAsync();
        return Ok(entity);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        logger.LogTrace($"Delete entity: {id}");
        using var _context = contextFactory.CreateDbContext();
        var entity = await _context.DummyEntities.FindAsync(id);
        if (entity == null) return NotFound();

        _context.DummyEntities.Remove(entity);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}