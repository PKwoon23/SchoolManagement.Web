using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Web.Models;
using SchoolManagement.Web.Services;

namespace SchoolManagement.Web.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<IActionResult> Index()
        {
            var students = await _studentService.GetAllStudents();
            return View(students);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (!ModelState.IsValid)
                return View(student);

            await _studentService.AddStudent(student);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var student = await _studentService.GetStudentById(id);
            if (student == null)
                return NotFound();

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Student student)
        {
            if (!ModelState.IsValid)
                return View(student);

            await _studentService.UpdateStudent(student);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _studentService.DeleteStudent(id);
            TempData["Success"] = "ลบสำเร็จ";
            return RedirectToAction(nameof(Index));
        }
    }
}
