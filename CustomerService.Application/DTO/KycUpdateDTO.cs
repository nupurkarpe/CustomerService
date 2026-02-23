using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.DTO
{
    public class KycUpdateDTO
    {
        public int? customerId { get; set; }
        public int? docTypeId { get; set; }
        public string? docRefNo { get; set; }
        public IFormFile? file { get; set; }
        public string? verificationStatus { get; set; }
    }
}
