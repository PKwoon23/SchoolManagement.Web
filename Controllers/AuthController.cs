using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Web.Services;

namespace SchoolManagement.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (_authService.Login(username, password))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "ชื่อผู้ใช้หรือรหัสผ่านไม่ถูกต้อง";
            return View();
        }
    }
}