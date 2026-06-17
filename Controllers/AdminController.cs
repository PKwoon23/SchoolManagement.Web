using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Web.Data;
using System.Threading.Tasks;

namespace SchoolManagement.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly SchoolDbContext _context;

        public AdminController(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.StudentCount = await _context.Students.CountAsync();
            ViewBag.TeacherCount = await _context.Teachers.CountAsync();
            ViewBag.CourseCount = await _context.Courses.CountAsync();
            ViewBag.FacultyCount = await _context.Faculties.CountAsync();

            return View();
        }
    }
}