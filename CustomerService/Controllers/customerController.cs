using CustomerService.Application.DTO;
using CustomerService.Application.Interface;
using CustomerService.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class customerController : ControllerBase
    {
        ICustomerRepo repo;
        public customerController(ICustomerRepo repo)
        {
            this.repo = repo;
        }
        [HttpPost]
        public async Task<IActionResult> AddCustomer(CustomerAddDTO dto)
        {
            var res = await repo.AddCustomer(dto);
            return Ok(ApiResponse.Success("Customer Details added successfully", res));
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> FetchCustomerById(int customerId)
        {
            var res = await repo.FetchCustomerById(customerId);
            return Ok(ApiResponse.Success("Customer Details Fetched successfully", res));
        }

        [HttpPatch("{customerId}")]
        public async Task<IActionResult> UpdateCustomer(int customerId, CustomerUpdateDTO dto)
        {
            var res = await repo.UpdateCustomer(customerId, dto);
            return Ok(ApiResponse.Success("Customer Details Updated successfully", res));
        }

        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteCustomer(int customerId)
        {
            var res = await repo.DeleteCustomer(customerId);
            return Ok(ApiResponse.Success("Customer Details Deleted successfully", res));
        }

        [HttpGet]
        public async Task<IActionResult> FetchAllCustomer(int page = 1, int pageSize = 10, string? name = null)
        {
            var res = await repo.FetchAllCustomer(page, pageSize, name);
            if (res.Items.Count == 0)
            {
                return Ok(ApiResponse.Success("No Customer found ", res));
            }
            return Ok(ApiResponse.Success("Customers fetched successfully", res));
        }
    }
}
