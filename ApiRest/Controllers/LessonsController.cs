using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CoursesOnline.Application.Interfaces;
using CoursesOnline.Application.DTOs;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LessonsController : ControllerBase
{
    private readonly ILessonService _lessonService;

    public LessonsController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    // GET: api/lessons/course/{courseId}
    [HttpGet("course/{courseId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByCourse(Guid courseId)
    {
        var lessons = await _lessonService.GetByCourseAsync(courseId);
        return Ok(lessons);
    }

    // POST: api/lessons
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateLessonDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var lesson = await _lessonService.CreateAsync(dto);
            return Ok(lesson);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT: api/lessons/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLessonDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _lessonService.UpdateAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Lección no encontrada" });
        }
    }

    // PATCH: api/lessons/{id}/reorder
    [HttpPatch("{id}/reorder")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Reorder(Guid id, [FromQuery] int newOrder)
    {
        if (newOrder < 1)
            return BadRequest(new { message = "El orden debe ser mayor a 0" });

        try
        {
            await _lessonService.ReorderAsync(id, newOrder);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Lección no encontrada" });
        }
    }

    // DELETE: api/lessons/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _lessonService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Lección no encontrada" });
        }
    }
}