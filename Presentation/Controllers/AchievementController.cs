using AutoMapper;
using Business.Dtos.Achievement;
using Common.Entities;
using Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class AchievementsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public AchievementsController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // Create a new achievement
    [HttpPost]
    public async Task<IActionResult> CreateAchievement([FromForm] CreateAchievementDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var newAchievement = _mapper.Map<Achievement>(dto);
        _context.Achievements.Add(newAchievement);

        try
        {
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAchievementById), new { id = newAchievement.Id }, newAchievement);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    // Get all achievements
    [HttpGet]
    public async Task<IActionResult> GetAllAchievements()
    {
        var achievements = await _context.Achievements.ToListAsync();
        return Ok(achievements);
    }

    // Get an achievement by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAchievementById(int id)
    {
        var achievement = await _context.Achievements.FindAsync(id);
        if (achievement == null)
            return NotFound();

        return Ok(achievement);
    }

    // Update an achievement
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAchievement(int id, [FromForm] UpdateAchievementDto dto)
    {
        var achievement = await _context.Achievements.FindAsync(id);
        if (achievement == null)
            return NotFound();

        var updatedAchievement = _mapper.Map(dto, achievement);
        _context.Achievements.Update(updatedAchievement);

        await _context.SaveChangesAsync();
        return Ok(achievement);
    }

    // Delete an achievement
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAchievement(int id)
    {
        var achievement = await _context.Achievements.FindAsync(id);
        if (achievement == null)
            return NotFound();

        _context.Achievements.Remove(achievement);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
