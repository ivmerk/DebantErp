using DebantErp.DAL.Models;

namespace DebantErp.DAL
{
    public interface IGradeDAL : IBaseDAL<GradeModel>
    {
        // Все версии по номеру разряда (в т.ч. архивные).
        Task<List<GradeModel>> GetByGrade(int grade);
    }
}
