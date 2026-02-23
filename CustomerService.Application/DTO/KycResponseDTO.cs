using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.DTO
{
    public class KycResponseDTO
    {
        public int kycId { get; set; }
        public int customerId { get; set; }
        public int docTypeId { get; set; }
        public string typeName { get; set; }
        public string docRefNo { get; set; }
        public string filePath { get; set; }
        public string verificationStatus { get; set; }
    }
}
