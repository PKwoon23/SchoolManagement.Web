using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Web.Models;
using SchoolManagement.Web.Services;

namespace SchoolManagement.Web.Controllers
{
    public class MajorController : Controller
    {
        private readonly MajorService _majorService;

        public MajorController(MajorService majorService)
        {
            _majorService = majorService;
        }

        public async Task<IActionResult> Index()
        {
            var majors = await _majorService.GetAllMajors();
            return View(majors);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Faculties = await _majorService.GetAllFaculties();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Major major)
        {
            var result = await _majorService.AddMajor(major);
            if (result.IsPass != "1")
            {
                ModelState.AddModelError(string.Empty, result.Message);
                ViewBag.Faculties = await _majorService.GetAllFaculties();
                return View(major);
            }

            TempData["Success"] = "เพิ่มสำเร็จ";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var major = await _majorService.GetMajorById(id);
            if (major == null)
                return NotFound();

            ViewBag.Faculties = await _majorService.GetAllFaculties();
            return View(major);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Major major)
        {
            var result = await _majorService.UpdateMajor(major);
            if (result.IsPass != "1")
            {
                ModelState.AddModelError(string.Empty, result.Message);
                ViewBag.Faculties = await _majorService.GetAllFaculties();
                return View(major);
            }

            TempData["Success"] = "แก้ไขสำเร็จ";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _majorService.DeleteMajor(id);
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
