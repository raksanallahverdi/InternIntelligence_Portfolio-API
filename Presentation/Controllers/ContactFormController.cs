using AutoMapper;
using Business.Dtos.ContactForm;
using Common.Entities;
using Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ContactFormController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ContactFormController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

   
    [HttpPost]
    public async Task<IActionResult> SubmitContactForm([FromForm] CreateContactFormDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var newContactForm = _mapper.Map<ContactForm>(dto);
        _context.ContactForms.Add(newContactForm);

        try
        {
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllContactForms), new { id = newContactForm.Id }, newContactForm);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

   
    [HttpGet]
    
    public async Task<IActionResult> GetAllContactForms()
    {
        var contactForms = await _context.ContactForms.ToListAsync();
        return Ok(contactForms);
    }
}
