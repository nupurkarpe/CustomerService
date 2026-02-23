using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CustomerService.Domain.Models
{
    public class Kyc
    {
        [Key]
        public int kycId { get; set; }

        [ForeignKey("customerDetails")]
        public int customerId { get; set; }

        [ForeignKey("docType")]
        public int docTypeId { get; set; }
        public string docRefNo { get; set; }
        public string filePath { get; set; }
        public string verificationStatus { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;
        public int createdBy { get; set; }
        public DateTime? modifiedAt { get; set; }
        public int? modifiedBy { get; set; }
        public DateTime? deletedAt { get; set; }
        public int? deletedBy { get; set; }
        public CustomerDetails customerDetails { get; set; }
        public DocType docType { get; set; }
    }
}
