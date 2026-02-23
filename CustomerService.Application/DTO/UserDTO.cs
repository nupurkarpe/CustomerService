using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CustomerService.Application.DTO
{
    public class UserDTO
    {
        [JsonPropertyName("id")]
        public int userId { get; set; }
        public string name { get; set; }
        public string email { get; set; }
    }
}
