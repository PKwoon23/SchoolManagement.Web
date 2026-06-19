using Microsoft.EntityFrameworkCore;
using SchoolManagement.Web.Data;
using SchoolManagement.Web.Models;

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
            var list = await _context.Students
                .FromSqlInterpolated($"EXEC GetStudentById @Id = {id}")
                .ToListAsync();

            return list.FirstOrDefault();
        }

        public async Task<SpResult> AddStudent(Student student)
        {
            var results = await _context.Database
                .SqlQuery<SpResult>($"""
                    EXEC [dbo].[AddStudent]
                        @Code = {student.StudentCode},
                        @FirstName = {student.FirstName ?? string.Empty},
                        @LastName = {student.LastName ?? string.Empty},
                        @Email = {student.Email ?? string.Empty},
                        @Phone = {student.Phone ?? string.Empty},
                        @EntranceYear = {student.EntranceYear},
                        @MajorId = {student.MajorId}
                    """)
                .ToListAsync();

            return results.FirstOrDefault()
                ?? new SpResult { IsPass = "0", Message = "ดำเนินการไม่สำเร็จ" };
        }

        public async Task<SpResult> UpdateStudent(Student student)
        {
            var results = await _context.Database
                .SqlQuery<SpResult>($"""
                    EXEC [dbo].[UpdateStudent]
                        @Id = {student.StudentId},
                        @Code = {student.StudentCode},
                        @FirstName = {student.FirstName ?? string.Empty},
                        @LastName = {student.LastName ?? string.Empty},
                        @Email = {student.Email ?? string.Empty},
                        @Phone = {student.Phone ?? string.Empty},
                        @EntranceYear = {student.EntranceYear},
                        @MajorId = {student.MajorId}
                    """)
                .ToListAsync();

            return results.FirstOrDefault()
                ?? new SpResult { IsPass = "0", Message = "ดำเนินการไม่สำเร็จ" };
        }

        public async Task<SpResult> DeleteStudent(int id)
        {
            var results = await _context.Database
                .SqlQuery<SpResult>($"EXEC [dbo].[DeleteStudent] @Id = {id}")
                .ToListAsync();

            return results.FirstOrDefault()
                ?? new SpResult { IsPass = "0", Message = "ดำเนินการไม่สำเร็จ" };
        }
    }
}
