using SchoolManagement.Web.Data;
using SchoolManagement.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace SchoolManagement.Web.Services
{
    public class StudentService
    {
        private readonly SchoolDbContext _context;

        public StudentService(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAllStudents()
        {
            return await _context.Students
                .FromSqlRaw("EXEC GetAllStudents")
                .ToListAsync();
        }

        public async Task<Student?> GetStudentById(int id)
        {
            return await _context.Students
                .FromSqlInterpolated($"EXEC GetStudentById {id}")
                .FirstOrDefaultAsync();
        }

        public async Task AddStudent(Student student)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC AddStudent @Code = {0}, @FirstName = {1}, @LastName = {2}, @Email = {3}, @Phone = {4}, @EntranceYear = {5}, @MajorId = {6}",
                    student.StudentCode, student.FirstName, student.LastName,
                    student.Email, student.Phone, student.EntranceYear, student.MajorId);
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task UpdateStudent(Student student)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC UpdateStudent @Id = {0}, @Code = {1}, @FirstName = {2}, @LastName = {3}, @Email = {4}, @Phone = {5}, @EntranceYear = {6}, @MajorId = {7}",
                    student.StudentId, student.StudentCode, student.FirstName, student.LastName,
                    student.Email, student.Phone, student.EntranceYear, student.MajorId);
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task DeleteStudent(int id)
        {
            try
            {
                await _context.Database
                    .ExecuteSqlInterpolatedAsync($"EXEC DeleteStudent @Id = {id}");
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
