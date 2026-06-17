using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Web.Models;
using System.Diagnostics;

namespace SchoolManagement.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}