using System.ComponentModel.DataAnnotations;

namespace BankManagementSystemVersionFinal1.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        public int Cin { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int PhoneNumber { get; set; }
        public string Mail { get; set; }

        public BankBranch BankBranch { get; set; }
        public List<Account> Accounts { get; set; }

        public Customer() { }
        public Customer(int cin, string name, String address, int phonenumber, string mail, BankBranch bankbranch)
        {
            Cin = cin;
            Name = name;
            Address = address;
            PhoneNumber = phonenumber;
            Mail = mail;
            Accounts = new List<Account>();
            BankBranch = bankbranch;
        }

        public void AddAccount(Account account) { }
    }
}
