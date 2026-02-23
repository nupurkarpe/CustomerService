using CustomerService.Application.DTO;
using CustomerService.Application.Interface;
using CustomerService.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class docTypeController : ControllerBase
    {
        IDocTypeRepo repo;
        public docTypeController(IDocTypeRepo repo)
        {
            this.repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> AddDocType(DocTypeAddDTO dto)
        {
            var res = await repo.AddDocType(dto);
            return Ok(ApiResponse.Success("Document Type added successfully", res));
        }

        [HttpGet("{docTypeId}")]
        public async Task<IActionResult> FetchCustomerById(int docTypeId)
        {
            var res = await repo.FetchDocTypeById(docTypeId);
            return Ok(ApiResponse.Success("Document Type Details Fetched successfully", res));
        }

        [HttpPatch("{docTypeId}")]
        public async Task<IActionResult> UpdateCustomer(int docTypeId, DocTypeAddDTO dto)
        {
            var res = await repo.UpdateDocType(docTypeId, dto);
            return Ok(ApiResponse.Success("Document Type Details Updated successfully", res));
        }

        [HttpDelete("{docTypeId}")]
        public async Task<IActionResult> DeleteCustomer(int docTypeId)
        {
            var res = await repo.DeleteDocType(docTypeId);
            return Ok(ApiResponse.Success("Document Type  Deleted successfully", res));
        }

        [HttpGet]
        public async Task<IActionResult> FetchAllDocType(int page = 1, int pageSize = 10, string? verificationStatus = null)
        {
            var res = await repo.FetchAllDocType(page, pageSize, verificationStatus);
            if (res.Items.Count == 0)
            {
                return Ok(ApiResponse.Success("No Doc type found ", res));
            }
            return Ok(ApiResponse.Success("Document Types Fetched successfully", res));
        }
    }
}
