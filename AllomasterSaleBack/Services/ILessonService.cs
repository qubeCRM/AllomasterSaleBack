using System.Collections.Generic;
using System.Threading.Tasks;
using AlloMasterSale.Data;

public interface ILessonService
{
    Task<IEnumerable<Lesson>> GetAllLessonsAsync();
    Task<Lesson> CreateLessonAsync(Lesson lesson);
}