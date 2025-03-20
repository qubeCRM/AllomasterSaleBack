using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlloMasterSale.Data;

public class LessonService : ILessonService
{
    private readonly List<Lesson> _lessons = new();

    public async Task<IEnumerable<Lesson>> GetAllLessonsAsync()
    {
        return await Task.FromResult(_lessons);
    }

    public async Task<Lesson> CreateLessonAsync(Lesson lesson)
    {
        lesson.Id = _lessons.Count + 1;
        _lessons.Add(lesson);
        return await Task.FromResult(lesson);
    }
}