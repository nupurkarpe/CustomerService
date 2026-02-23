using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace CustomerService.Application.DTO
{
    public class KycAddDTO
    {
        public int customerId { get; set; }
        public int docTypeId { get; set; }
        public string docRefNo { get; set; }
        public IFormFile file { get; set; }
    }
}
