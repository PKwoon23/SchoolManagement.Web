using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Web.Data;
using SchoolManagement.Web.Models;

namespace SchoolManagement.Web.Services
{
    public class CourseService
    {
        private readonly SchoolDbContext _context;

        public CourseService(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAllCourses()
        {
            return await _context.Courses
                .FromSqlRaw("EXEC [dbo].[GetAllCourses]")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Course?> GetCourseById(int id)
        {
            var list = await _context.Courses
                .FromSqlInterpolated($"EXEC [dbo].[GetCourseById] @Id = {id}")
                .AsNoTracking()
                .ToListAsync();

            return list.FirstOrDefault();
        }

        public async Task<List<Teacher>> GetTeachersByCourseId(int courseId)
        {
            var param = new SqlParameter("@course_id", courseId);
            return await _context.Teachers
                .FromSqlRaw("EXEC [dbo].[GetTeachersByCourse] @course_id", param)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<SpResult> AddCourse(Course course)
        {
            var results = await _context.Database
                .SqlQuery<SpResult>($"EXEC [dbo].[AddCourse] @Name = {course.CourseName ?? string.Empty}, @Credit = {course.Credit}")
                .ToListAsync();

            return results.FirstOrDefault()
                ?? new SpResult { IsPass = "0", Message = "ดำเนินการไม่สำเร็จ" };
        }

        public async Task<SpResult> UpdateCourse(Course course)
        {
            var results = await _context.Database
                .SqlQuery<SpResult>($"EXEC [dbo].[UpdateCourse] @Id = {course.CourseId}, @Name = {course.CourseName ?? string.Empty}, @Credit = {course.Credit}")
                .ToListAsync();

            return results.FirstOrDefault()
                ?? new SpResult { IsPass = "0", Message = "ดำเนินการไม่สำเร็จ" };
        }

        public async Task<SpResult> DeleteCourse(int id)
        {
            var results = await _context.Database
                .SqlQuery<SpResult>($"EXEC [dbo].[DeleteCourse] @Id = {id}")
                .ToListAsync();

            return results.FirstOrDefault()
                ?? new SpResult { IsPass = "0", Message = "ดำเนินการไม่สำเร็จ" };
        }
    }
}
