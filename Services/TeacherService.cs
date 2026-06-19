using Microsoft.EntityFrameworkCore;
using SchoolManagement.Web.Data;
using SchoolManagement.Web.Models;

namespace SchoolManagement.Web.Services
{
    public class TeacherService
    {
        private readonly SchoolDbContext _context;

        public TeacherService(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<List<Teacher>> GetAllTeachers()
        {
            return await _context.Teachers
                .FromSqlRaw("EXEC GetAllTeachers")
                .ToListAsync();
        }

        public async Task<Teacher?> GetTeacherById(int id)
        {
            var list = await _context.Teachers
                .FromSqlInterpolated($"EXEC GetTeacherById @Id = {id}")
                .ToListAsync();

            return list.FirstOrDefault();
        }

        public async Task<SpResult> AddTeacher(Teacher teacher)
        {
            var results = await _context.Database
                .SqlQuery<SpResult>($"EXEC [dbo].[AddTeacher] @Name = {teacher.TeacherName ?? string.Empty}, @MajorId = {teacher.MajorId}")
                .ToListAsync();

            return results.FirstOrDefault()
                ?? new SpResult { IsPass = "0", Message = "ดำเนินการไม่สำเร็จ" };
        }

        public async Task<SpResult> UpdateTeacher(Teacher teacher)
        {
            var results = await _context.Database
                .SqlQuery<SpResult>($"EXEC [dbo].[UpdateTeacher] @Id = {teacher.TeacherId}, @Name = {teacher.TeacherName ?? string.Empty}, @MajorId = {teacher.MajorId}")
                .ToListAsync();

            return results.FirstOrDefault()
                ?? new SpResult { IsPass = "0", Message = "ดำเนินการไม่สำเร็จ" };
        }

        public async Task<SpResult> DeleteTeacher(int id)
        {
            var results = await _context.Database
                .SqlQuery<SpResult>($"EXEC [dbo].[DeleteTeacher] @Id = {id}")
                .ToListAsync();

            return results.FirstOrDefault()
                ?? new SpResult { IsPass = "0", Message = "ดำเนินการไม่สำเร็จ" };
        }
    }
}
