using SchoolManagement.Web.Data;
using SchoolManagement.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

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
            return await _context.Teachers
                .FromSqlInterpolated($"EXEC GetTeacherById {id}")
                .FirstOrDefaultAsync();
        }

        public async Task AddTeacher(Teacher teacher)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC AddTeacher @Name = {0}, @MajorId = {1}",
                    teacher.TeacherName, teacher.MajorId);
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task UpdateTeacher(Teacher teacher)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC UpdateTeacher @Id = {0}, @Name = {1}, @MajorId = {2}",
                    teacher.TeacherId, teacher.TeacherName, teacher.MajorId);
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task DeleteTeacher(int id)
        {
            try
            {
                await _context.Database
                    .ExecuteSqlInterpolatedAsync($"EXEC DeleteTeacher @Id = {id}");
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
