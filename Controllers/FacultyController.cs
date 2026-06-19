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

        // ------------------------- Create ------------------------------//
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Faculty faculty)
        {
            var result = await _facultyService.AddFaculty(faculty);
            if (result.IsPass != "1")
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(faculty);
            }

            TempData["Success"] = "เพิ่มสำเร็จ";
            return RedirectToAction(nameof(Index));
        }

        // ------------------------- Edit ------------------------------//
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
            var result = await _facultyService.UpdateFaculty(faculty);
            if (result.IsPass != "1")
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(faculty);
            }

            TempData["Success"] = "แก้ไขสำเร็จ";
            return RedirectToAction(nameof(Index));
        }


        // ------------------------- Delete ------------------------------//
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _facultyService.DeleteFaculty(id);
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
