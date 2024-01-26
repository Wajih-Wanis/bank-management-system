using BankManagementSystemVersionFinal1.Data;
using BankManagementSystemVersionFinal1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
namespace BankManagementSystemVersionFinal1.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var accounts = _context.Accounts
                .Include(a => a.AccountHolder)
                .Include(a => a.Transactions)
                .Include(a => a.loans)
                .Include(a => a.Transfers)
                .ToList();
            return View(accounts);

        }
        public async Task<IActionResult> Details(int id)
        {
            var account = await _context.Accounts
                .Include(a => a.AccountHolder)
                .FirstOrDefaultAsync(a => a.AccountId == id);


            if (account == null)
            {
                return NotFound();
            }

            var transactions = await _context.Transactions
                .Include(t => t.Account)
                .Where(t => t.Account.AccountId == id)
                .ToListAsync();


            var deposits = new List<Transaction>();
            var withdrawals = new List<Transaction>();

            foreach (var transaction in transactions)
            {


                if (transaction.Description == "Deposit")
                {
                    deposits.Add(transaction);
                }
                else if (transaction.Description == "Withdraw")
                {
                    withdrawals.Add(transaction);
                }
            }
            var loans = await _context.Loans
                .Include(l => l.Account)
                .Where(l => l.Account.AccountId == id)
                .ToListAsync();

            var transfers = await _context.Transfers
                .Include(t => t.Sender)
                    .ThenInclude(t => t.AccountHolder)
                .Include(t => t.Receiver)
                    .ThenInclude(t => t.AccountHolder)
                .Where(t => t.Sender.AccountId == id || t.Receiver.AccountId == id)
                .ToListAsync();

            ViewBag.Deposits = deposits;
            ViewBag.Withdrawals = withdrawals;
            ViewBag.loans = loans;
            ViewBag.transfers = transfers;
            ViewBag.account = account;

            return View(account);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,Balance,AccountStatus")] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }
    }
}

