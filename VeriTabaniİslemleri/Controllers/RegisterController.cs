
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using VeriTabani;
using System.Diagnostics;
namespace VeriTabaniİslemleri.Controllers
{
    public class RegisterController : Controller
    {
        NorthwindContext context = new NorthwindContext();
        public IActionResult RegisterPage()
        {
            Employee register = new Employee();
            return View(register);

        }

        [HttpPost]
        [ActionName("Ekle")]
        public IActionResult Eklepost(Employee employee)
        {
            context.Employees.Add(employee);
            context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

    }
}
