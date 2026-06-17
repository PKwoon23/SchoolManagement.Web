using SchoolManagement.Web.Data;
using SchoolManagement.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

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
            var courses = await _context.Courses
              .FromSqlRaw("EXEC [dbo].[GetAllCourses]")
              .AsNoTracking()
              .ToListAsync();
            return courses;
        }

        public async Task<Course?> GetCourseById(int id)
        {
            var result = await _context.Courses
              .FromSqlRaw("EXEC [dbo].[GetCourseById] @p0", id)
              .AsNoTracking()
              .ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task<List<Teacher>> GetTeachersByCourseId(int courseId)
        {
            var param = new SqlParameter("@course_id", courseId);
            return await _context.Teachers
                .FromSqlRaw("EXEC [dbo].[GetTeachersByCourse] @course_id", param)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddCourse(Course course)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC AddCourse @Name = {0}, @Credit = {1}",
                    course.CourseName, course.Credit);
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task UpdateCourse(Course course)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC UpdateCourse @Id = {0}, @Name = {1}, @Credit = {2}",
                    course.CourseId, course.CourseName, course.Credit);
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        public async Task DeleteCourse(int id)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC DeleteCourse @Id = {0}", id);
            }
            catch (SqlException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
