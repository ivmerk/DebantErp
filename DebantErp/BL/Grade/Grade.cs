using DebantErp.DAL;
using DebantErp.DAL.Models;
using DebantErp.Dtos;
using DebantErp.Rdos;

namespace DebantErp.BL.Grade
{
    public class Grade : IGrade
    {
        private readonly IGradeDAL _gradeDAL;

        public Grade(IGradeDAL gradeDAL)
        {
            _gradeDAL = gradeDAL;
        }

        public async Task<List<GradeRdo>> GetGrades()
        {
            var grades = await _gradeDAL.Get(); // действующие (is_actual = true)
            return grades.OrderBy(g => g.Grade).Select(Map).ToList();
        }

        public async Task<GradeRdo> GetGrade(int id)
        {
            var grade = await _gradeDAL.Get(id);
            return Map(grade);
        }

        public async Task<List<GradeRdo>> GetHistory(int grade)
        {
            var all = await _gradeDAL.GetByGrade(grade);
            return all
                .OrderByDescending(g => g.CreatedAt)
                .Select(Map)
                .ToList();
        }

        public async Task<int> Create(CreateGradeDto dto)
        {
            // Одна действующая ставка на номер разряда: если уже есть —
            // версионируем (старую в историю).
            var active = (await _gradeDAL.GetByGrade(dto.Grade))
                .FirstOrDefault(g => g.IsActual);
            if (active != null && active.Id != 0)
            {
                await _gradeDAL.Delete(active.Id);
            }

            return await _gradeDAL.Create(new GradeModel
            {
                Grade = dto.Grade,
                DailyRate = dto.DailyRate,
                EffectiveDate = dto.EffectiveDate,
            });
        }

        public async Task<int> Change(int id, ChangeGradeDto dto)
        {
            var old = await _gradeDAL.Get(id);
            if (old.Id == 0)
            {
                return 0;
            }

            // Версионирование: старую помечаем неактуальной (уходит в историю),
            // вставляем новую действующую версию с тем же номером разряда.
            await _gradeDAL.Delete(id);
            return await _gradeDAL.Create(new GradeModel
            {
                Grade = old.Grade,
                DailyRate = dto.DailyRate,
                EffectiveDate = dto.EffectiveDate,
            });
        }

        public Task<int> Delete(int id) => _gradeDAL.Delete(id);

        private static GradeRdo Map(GradeModel g) => new()
        {
            Id = g.Id,
            Grade = g.Grade,
            DailyRate = g.DailyRate,
            EffectiveDate = g.EffectiveDate,
            IsActual = g.IsActual,
            CreatedAt = g.CreatedAt,
        };
    }
}
