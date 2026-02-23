using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CustomerService.Domain.Models
{
    public class CustomerDetails
    {
        [Key]
        public int customerId { get; set; }
        public int userId { get; set; }
        public string mobile { get; set; }
        public string aadhar { get; set; }
        public string pan { get; set; }
        public DateTime? dob { get; set; }
        public int? age { get; set; }
        public string gender { get; set; }
        public string employmentType { get; set; }
        public decimal? monthlyIncome { get; set; }
        public string status { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;
        public int createdBy { get; set; }
        public DateTime? modifiedAt { get; set; }
        public int? modifiedBy { get; set; }
        public DateTime? deletedAt { get; set; }
        public int? deletedBy { get; set; }
        public Kyc kyc { get; set; }
    }
}
