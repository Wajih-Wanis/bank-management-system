using BankManagementSystemVersionFinal1.Data;
using BankManagementSystemVersionFinal1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BankManagementSystemVersionFinal1.Controllers.Customer
{
    [Route("customer/Login")]
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userId, string password)
        {
            // Check if a login of given number exists in the database
            var login = await _context.CustomerModels.FirstOrDefaultAsync(x => x.LoginId.Equals(userId));

            if (login == null || login.Password != password)
            {
                ModelState.AddModelError("LoginFailed", "Unsuccessful login attempt");
                return View();
            }

            var customer = await _context.CustomerModels.FirstOrDefaultAsync(x => x.LoginId == login.LoginId);

            // Set session customer
            HttpContext.Session.SetInt32(nameof(CustomerModel.LoginId), login.LoginId);
            HttpContext.Session.SetString(nameof(CustomerModel.Email), customer.Email);

            return RedirectToAction("Index", "Accounts");
        }

        [Route("Logout")]
        public IActionResult Logout()
        {
            // Logout customer.
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
