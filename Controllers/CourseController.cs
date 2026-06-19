using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Web.Models;
using SchoolManagement.Web.Services;

namespace SchoolManagement.Web.Controllers
{
    public class CourseController : Controller
    {
        private readonly CourseService _courseService;

        public CourseController(CourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllCourses();
            return View(courses);
        }

        public async Task<IActionResult> Details(int id)
        {
            var course = await _courseService.GetCourseById(id);
            if (course == null)
                return NotFound();

            return View(course);
        }

        public async Task<IActionResult> List_Teachers(int id)
        {
            var course = await _courseService.GetCourseById(id);
            if (course == null)
                return NotFound();

            var teachers = await _courseService.GetTeachersByCourseId(id);
            ViewBag.CourseName = course.CourseName;
            ViewBag.CourseId = course.CourseId;
            return View(teachers ?? new List<Teacher>());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            var result = await _courseService.AddCourse(course);
            if (result.IsPass != "1")
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(course);
            }

            TempData["Success"] = "เพิ่มสำเร็จ";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var course = await _courseService.GetCourseById(id);
            if (course == null)
                return NotFound();

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Course course)
        {
            var result = await _courseService.UpdateCourse(course);
            if (result.IsPass != "1")
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(course);
            }

            TempData["Success"] = "แก้ไขสำเร็จ";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _courseService.DeleteCourse(id);
            if (result.IsPass != "1")
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "ลบสำเร็จ";
            return RedirectToAction(nameof(Index));
        }
    }
}
