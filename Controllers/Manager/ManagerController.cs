using BankManagementSystemVersionFinal1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankManagementSystemVersionFinal1.Controllers.Manager
{
    [Route("manager")]
    public class ManagerController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ManagerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public ViewResult Index()
        {
            return View("~/Views/Manager/Index.cshtml");
        }

        [HttpGet("loans")]
        public ViewResult getLoans()
        {
            var loans = _context.Loans.ToList();
            return View("~/Views/Manager/View_Loans.cshtml", loans);
        }

        [HttpGet("loans/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans.Include(l=>l.Account).FirstOrDefaultAsync(l=>l.LoanId == id);
            if (loan == null)
            {
                return NotFound();
            }
            return View("~/Views/Manager/View_Loan_Details.cshtml", loan);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Loans == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }
            return View("~/Views/Manager/View_Loan_Details.cshtml", loan);
        }

    }
}
