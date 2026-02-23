using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CustomerService.Domain.Models
{
    public class DocType
    {
        [Key]
        public int docTypeId { get; set; }
        public string typeName { get; set; }
        public DateTime createdAt { get; set; } = DateTime.Now;
        public int createdBy { get; set; }
        public DateTime? modifiedAt { get; set; }
        public int? modifiedBy { get; set; }
        public DateTime? deletedAt { get; set; }
        public int? deletedBy { get; set; }
        public Kyc kyc { get; set; }
    }
}
