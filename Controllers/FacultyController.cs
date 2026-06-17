using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Web.Models;
using SchoolManagement.Web.Services;

namespace SchoolManagement.Web.Controllers
{
    public class FacultyController : Controller
    {
        private readonly FacultyService _facultyService;

        public FacultyController(FacultyService facultyService)
        {
            _facultyService = facultyService;
        }

        public async Task<IActionResult> Index()
        {
            var faculties = await _facultyService.GetAllFaculties();
            return View(faculties);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Faculty faculty)
        {
            if (!ModelState.IsValid)
                return View(faculty);

            await _facultyService.AddFaculty(faculty);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var faculty = await _facultyService.GetFacultyById(id);
            if (faculty == null)
                return NotFound();

            return View(faculty);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Faculty faculty)
        {
            if (!ModelState.IsValid)
                return View(faculty);

            await _facultyService.UpdateFaculty(faculty);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _facultyService.DeleteFaculty(id);
            TempData["Success"] = "ลบสำเร็จ";
            return RedirectToAction(nameof(Index));
        }
    }
}
