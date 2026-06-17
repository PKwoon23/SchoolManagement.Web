using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Web.Data;
using SchoolManagement.Web.Models;

namespace SchoolManagement.Web.Services
{
    public class FacultyService
    {
        private readonly SchoolDbContext _context;

        public FacultyService(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<List<Faculty>> GetAllFaculties()
        {
            return await _context.Faculties
                .FromSqlRaw("EXEC GetAllFaculties")
                .ToListAsync();
        }

        public async Task<Faculty?> GetFacultyById(int id)
        {
            return await _context.Faculties
                .FromSqlInterpolated($"EXEC GetFacultyById @Id = {id}") 
                .FirstOrDefaultAsync();
        }

        public async Task AddFaculty(Faculty faculty)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC [dbo].[AddFaculty] @Name = {faculty.FacultyName ?? string.Empty}");
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task UpdateFaculty(Faculty faculty)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC UpdateFaculty @Id = {faculty.FacultyId}, @Name = {faculty.FacultyName}");
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

       
        public async Task DeleteFaculty(int id)
        {
            try
            {
                await _context.Database
                    .ExecuteSqlInterpolatedAsync($"EXEC DeleteFaculty @Id = {id}");
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}