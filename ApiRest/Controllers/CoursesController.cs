using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CoursesOnline.Application.Interfaces;
using CoursesOnline.Application.DTOs;
using CoursesOnline.Domain.Enums;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    // GET: api/courses/search
    [HttpGet("search")]
    [AllowAnonymous]
    public async Task<IActionResult> Search(
        [FromQuery] string? query,
        [FromQuery] CourseStatus? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 10;

        var courses = await _courseService.SearchAsync(query, status, page, pageSize);
        return Ok(courses);
    }

    // GET: api/courses/{id}/summary
    [HttpGet("{id}/summary")]
    [AllowAnonymous]
    public async Task<IActionResult> GetSummary(Guid id)
    {
        var summary = await _courseService.GetSummaryAsync(id);

        if (summary == null)
            return NotFound(new { message = "Curso no encontrado" });

        return Ok(summary);
    }

    // POST: api/courses
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateCourseDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var course = await _courseService.CreateAsync(dto);
        
        // Retorna el curso creado directamente con status 201 Created
        return CreatedAtAction(
            nameof(GetSummary), 
            new { id = course.Id }, 
            course
        );
    }

    // PUT: api/courses/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCourseDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _courseService.UpdateAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Curso no encontrado" });
        }
    }

    // POST: api/courses/{id}/publish
    [HttpPost("{id}/publish")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Publish(Guid id)
    {
        try
        {
            await _courseService.PublishAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Curso no encontrado" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // POST: api/courses/{id}/unpublish
    [HttpPost("{id}/unpublish")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Unpublish(Guid id)
    {
        try
        {
            await _courseService.UnpublishAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Curso no encontrado" });
        }
    }

    // DELETE: api/courses/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _courseService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Curso no encontrado" });
        }
    }
}