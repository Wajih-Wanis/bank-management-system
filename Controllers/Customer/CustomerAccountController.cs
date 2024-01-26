using BankManagementSystemVersionFinal1.Data;
using BankManagementSystemVersionFinal1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static BankManagementSystemVersionFinal1.Models.Loan;

namespace BankManagementSystemVersionFinal1.Controllers.Customer
{
    public class CustomerAccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerAccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var accounts = _context.Accounts
                .Include(a => a.AccountHolder)
                .Include(a => a.Transactions)
                .Where(a => a.AccountHolder.CustomerId.Equals(customerId))
                .ToList();
            return View(accounts);
        }

        public async Task<IActionResult> Details(int id)
        {
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var account = await _context.Accounts
                .Include(a => a.AccountHolder)
                .FirstOrDefaultAsync(a => a.AccountId == id && a.AccountHolder.CustomerId.Equals(customerId));

            if (account == null)
            {
                return NotFound();
            }

            var transactions = await _context.Transactions
                .Include(t => t.Account)
                .Where(t => t.Account.AccountId == id)
                .ToListAsync();

            ViewBag.Transactions = transactions;
            ViewBag.Account = account;

            return View(account);
        }

        public IActionResult CreateLoan(int id)
        {
            return View(new Loan { Account = new Account { AccountId = id } });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLoan([Bind("Account,Amount")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                loan.LoanStatus = LoanStatusEnum.AwaitingApproval;
                _context.Add(loan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = loan.Account.AccountId });
            }
            return View(loan);
        }

        //get
        public IActionResult CreateTransfer(int id)
        {
            return View(new Transfer { Sender = new Account { AccountId = id } });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTransfer([Bind("SenderAccountId,ReceiverAccountId,Amount")] Transfer transfer)
        {
            if (ModelState.IsValid)
            {
                var senderAccount = await _context.Accounts.FindAsync(transfer.Sender);
                var receiverAccount = await _context.Accounts.FindAsync(transfer.ReceiverId);

                if (senderAccount == null || receiverAccount == null)
                {
                    return NotFound();
                }

                senderAccount.Balance -= transfer.Amount;
                receiverAccount.Balance += transfer.Amount;

                _context.Transfers.Add(transfer);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Details), new { id = transfer.Sender });
            }

            return View(transfer);
        }

    }
}
