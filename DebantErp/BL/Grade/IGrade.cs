using DebantErp.Dtos;
using DebantErp.Rdos;

namespace DebantErp.BL.Grade
{
    public interface IGrade
    {
        Task<List<GradeRdo>> GetGrades();              // действующие разряды
        Task<GradeRdo> GetGrade(int id);               // версия по id (в т.ч. архивная)
        Task<List<GradeRdo>> GetHistory(int grade);    // все версии номера разряда
        Task<int> Create(CreateGradeDto dto);          // новый действующий разряд
        Task<int> Change(int id, ChangeGradeDto dto);  // новая версия (старая в историю)
        Task<int> Delete(int id);                      // мягкое удаление
    }
}
