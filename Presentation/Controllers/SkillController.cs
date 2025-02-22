using AutoMapper;
using Business.Dtos.Skill;
using Common.Entities;
using Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class SkillsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public SkillsController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // Create a new skill
    [HttpPost]
    public async Task<IActionResult> CreateSkill([FromForm] CreateSkillDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var newSkill = _mapper.Map<Skill>(dto);
        _context.Skills.Add(newSkill);

        try
        {
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSkillById), new { id = newSkill.Id }, newSkill);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    // Get all skills
    [HttpGet]
    public async Task<IActionResult> GetAllSkills()
    {
        var skills = await _context.Skills.ToListAsync();
        return Ok(skills);
    }

    // Get a skill by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSkillById(int id)
    {
        var skill = await _context.Skills.FindAsync(id);
        if (skill == null)
            return NotFound();

        return Ok(skill);
    }

    // Update a skill
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSkill(int id, [FromForm] UpdateSkillDto dto)
    {
        var skill = await _context.Skills.FindAsync(id);
        if (skill == null)
            return NotFound();

        var updatedSkill = _mapper.Map(dto, skill);
        _context.Skills.Update(updatedSkill);

        await _context.SaveChangesAsync();
        return Ok(skill);
    }

    // Delete a skill
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSkill(int id)
    {
        var skill = await _context.Skills.FindAsync(id);
        if (skill == null)
            return NotFound();

        _context.Skills.Remove(skill);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
