using Microsoft.EntityFrameworkCore;
using SchoolManagement.Web.Data;
using SchoolManagement.Web.Models;

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
            var list = await _context.Majors
                .FromSqlInterpolated($"EXEC GetMajorById @Id = {id}")
                .ToListAsync();

            return list.FirstOrDefault();
        }

        public async Task<List<Faculty>> GetAllFaculties()
        {
            return await _context.Faculties
                .FromSqlRaw("EXEC GetAllFaculties")
                .ToListAsync();
        }

        public async Task<SpResult> AddMajor(Major major)
        {
            var results = await _context.Database
                .SqlQuery<SpResult>($"EXEC [dbo].[AddMajor] @Name = {major.MajorName ?? string.Empty}, @FacultyId = {major.FacultyId}")
                .ToListAsync();

            return results.FirstOrDefault()
                ?? new SpResult { IsPass = "0", Message = "ดำเนินการไม่สำเร็จ" };
        }

        public async Task<SpResult> UpdateMajor(Major major)
        {
            var results = await _context.Database
                .SqlQuery<SpResult>($"EXEC [dbo].[UpdateMajor] @Id = {major.MajorId}, @Name = {major.MajorName ?? string.Empty}, @FacultyId = {major.FacultyId}")
                .ToListAsync();

            return results.FirstOrDefault()
                ?? new SpResult { IsPass = "0", Message = "ดำเนินการไม่สำเร็จ" };
        }

        public async Task<SpResult> DeleteMajor(int id)
        {
            var results = await _context.Database
                .SqlQuery<SpResult>($"EXEC [dbo].[DeleteMajor] @Id = {id}")
                .ToListAsync();

            return results.FirstOrDefault()
                ?? new SpResult { IsPass = "0", Message = "ดำเนินการไม่สำเร็จ" };
        }
    }
}
