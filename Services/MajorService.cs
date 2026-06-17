using SchoolManagement.Web.Data;
using SchoolManagement.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace SchoolManagement.Web.Services
{
    public class MajorService
    {
        private readonly SchoolDbContext _context;

        public MajorService(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<List<Major>> GetAllMajors()
        {
            return await _context.Majors
                .FromSqlRaw("EXEC GetAllMajors")
                .ToListAsync();
        }

        public async Task<Major?> GetMajorById(int id)
        {
            return await _context.Majors
                .FromSqlInterpolated($"EXEC GetMajorById {id}")
                .FirstOrDefaultAsync();
        }

        public async Task<List<Faculty>> GetAllFaculties()
        {
            return await _context.Faculties
                .FromSqlRaw("EXEC GetAllFaculties")
                .ToListAsync();
        }

        public async Task AddMajor(Major major)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC AddMajor @Name = {0}, @FacultyId = {1}",
                    major.MajorName, major.FacultyId);
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task UpdateMajor(Major major)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC UpdateMajor @Id = {0}, @Name = {1}, @FacultyId = {2}",
                    major.MajorId, major.MajorName, major.FacultyId);
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task DeleteMajor(int id)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"EXEC DeleteMajor @Id = {id}");
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
