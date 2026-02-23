using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.DTO
{
    public class CustomerResponseDTO
    {
        public int customerId { get; set; }
        public int userId { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string aadhar { get; set; }
        public string pan { get; set; }
        public DateTime? dob { get; set; }
        public int? age { get; set; }
        public string gender { get; set; }
        public string employmentType { get; set; }
        public decimal? monthlyIncome { get; set; }
        public string status { get; set; }
        public DateTime createdAt { get; set; }

    }
}
