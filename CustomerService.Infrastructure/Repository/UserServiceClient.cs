using CustomerService.Application.DTO;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace CustomerService.Infrastructure.Repository
{
    public class UserServiceClient
    {
        private readonly HttpClient client;

        public UserServiceClient(HttpClient client)
        {
            this.client = client;
        }

        //public async Task<bool> UserExists(int userId)
        //{
        //    var response = await client.GetFromJsonAsync<bool>($"user/{userId}");
        //    return response;
        //}

        public async Task<UserDTO> GetUserById(int userId)
        {
            var response = await client.GetFromJsonAsync<JsonElement>($"/api/User/{userId}");
            return response.GetProperty("data").Deserialize<UserDTO>();
        }

        public async Task<List<UserDTO>> GetAllUser()
        {
            var response = await client.GetFromJsonAsync<JsonElement>($"/api/User");
            return response.GetProperty("data").Deserialize<List<UserDTO>>();
        }
    }
}
