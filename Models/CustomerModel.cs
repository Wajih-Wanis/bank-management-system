using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace BankManagementSystemVersionFinal1.Models
{
    public class CustomerModel
    {
        [Key,Required]
        public int LoginId { get; set; }
        [Required]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }

}
