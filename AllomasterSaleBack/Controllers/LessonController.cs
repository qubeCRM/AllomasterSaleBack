using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlloMasterSale.Data;

[ApiController]
[Route("api/lessons")]
public class LessonController : ControllerBase
{
    private readonly ILessonService _lessonService;

    public LessonController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    // Получение всех видео (доступно всем)
    [HttpGet]
    public async Task<IActionResult> GetLessons()
    {
        var lessons = await _lessonService.GetAllLessonsAsync();
        return Ok(lessons);
    }

    // Добавление нового видео (только для менеджеров)
    [HttpPost]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> CreateLesson([FromBody] Lesson lesson)
    {
        if (lesson == null || string.IsNullOrEmpty(lesson.YoutubeLink))
        {
            return BadRequest(new { message = "Некорректные данные" });
        }

        var createdLesson = await _lessonService.CreateLessonAsync(lesson);
        return CreatedAtAction(nameof(GetLessons), new { id = createdLesson.Id }, createdLesson);
    }
}