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
            var list = await _context.Faculties
                .FromSqlInterpolated($"EXEC GetFacultyById @Id = {id}")
                .ToListAsync();

            return list.FirstOrDefault();
        }


        public async Task<SpResult> AddFaculty(Faculty faculty)
        {
            var results = await _context.Database
                .SqlQuery<SpResult>($"EXEC [dbo].[AddFaculty] @Name = {faculty.FacultyName ?? string.Empty}")
                .ToListAsync();

            return results.FirstOrDefault()
                ?? new SpResult { IsPass = "0", Message = "ดำเนินการไม่สำเร็จ" };
        }

        public async Task<SpResult> UpdateFaculty(Faculty faculty)
        {
            var results = await _context.Database
                .SqlQuery<SpResult>($"EXEC [dbo].[UpdateFaculty] @Id = {faculty.FacultyId}, @Name = {faculty.FacultyName}")
                .ToListAsync();

            return results.FirstOrDefault()
                ?? new SpResult { IsPass = "0", Message = "ดำเนินการไม่สำเร็จ" };
        }

        public async Task<SpResult> DeleteFaculty(int id)
        {
            var results = await _context.Database
                .SqlQuery<SpResult>($"EXEC [dbo].[DeleteFaculty] @Id = {id}")
                .ToListAsync();

            return results.FirstOrDefault()
                ?? new SpResult { IsPass = "0", Message = "ดำเนินการไม่สำเร็จ" };
        }
    }
}
