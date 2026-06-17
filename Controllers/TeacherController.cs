using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Web.Models;
using SchoolManagement.Web.Services;

namespace SchoolManagement.Web.Controllers
{
    public class TeacherController : Controller
    {
        private readonly TeacherService _teacherService;
        private readonly MajorService _majorService;

        public TeacherController(TeacherService teacherService, MajorService majorService)
        {
            _teacherService = teacherService;
            _majorService = majorService;
        }

        public async Task<IActionResult> Index()
        {
            var teachers = await _teacherService.GetAllTeachers();
            return View(teachers);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Majors = await _majorService.GetAllMajors();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Majors = await _majorService.GetAllMajors();
                return View(teacher);
            }

            await _teacherService.AddTeacher(teacher);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var teacher = await _teacherService.GetTeacherById(id);
            if (teacher == null)
                return NotFound();

            ViewBag.Majors = await _majorService.GetAllMajors();
            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Majors = await _majorService.GetAllMajors();
                return View(teacher);
            }

            await _teacherService.UpdateTeacher(teacher);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _teacherService.DeleteTeacher(id);
            TempData["Success"] = "ลบสำเร็จ";
            return RedirectToAction(nameof(Index));
        }
    }
}
