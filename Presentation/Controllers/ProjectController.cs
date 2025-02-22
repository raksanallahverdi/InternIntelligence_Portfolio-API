using AutoMapper;
using Business.Dtos.Project;
using Common.Entities;
using Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ProjectController(AppDbContext context,
        IMapper mapper
        )
    {
        _context = context;
        _mapper = mapper;
    }


    [HttpPost]
    public async Task<IActionResult> CreateProject([FromForm] CreateProjectDto Dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

       
        var newProject = _mapper.Map<Project>(Dto);

        
        _context.Projects.Add(newProject);

        try
        {
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProjectById), new { id = newProject.Id }, newProject);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    
    }

  
    [HttpGet]
    public async Task<IActionResult> GetAllProjects()
    {
        var projects = await _context.Projects.ToListAsync();
        return Ok(projects);
    }

   
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProjectById(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
            return NotFound();

        return Ok(project);
    }

  
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(int id, [FromForm] UpdateProjectDto Dto)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
            return NotFound();
       

       
        var updatedProject = _mapper.Map(Dto,project);


        _context.Projects.Update(updatedProject);

        await _context.SaveChangesAsync();
        return Ok(project);
    }

  
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
            return NotFound();

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
