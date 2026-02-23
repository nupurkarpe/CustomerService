using CustomerService.Application.DTO;
using CustomerService.Application.Interface;
using CustomerService.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class kycController : ControllerBase
    {
        IKycRepo repo;
        public kycController(IKycRepo repo)
        {
            this.repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> AddKyc(KycAddDTO dto)
        {
            var res = await repo.AddKyc(dto);
            return Ok(ApiResponse.Success("Kyc added successfully", res));
        }

        [HttpPatch("{kycId}")]
        public async Task<IActionResult> UpdateKyc(int kycId, KycUpdateDTO dto)
        {
            var res = await repo.UpdateKyc(kycId, dto);
            return Ok(ApiResponse.Success("Kyc updated successfully", res));
        }


        [HttpGet("{kycId}")]
        public async Task<IActionResult> FetchKycById(int kycId)
        {
            var res = await repo.GetKycByID(kycId);
            return Ok(ApiResponse.Success("Kyc details fetched successfully", res));
        }

        [HttpDelete("{kycId}")]
        public async Task<IActionResult> DeleteKyc(int kycId)
        {
            var res = await repo.DeleteKyc(kycId);
            return Ok(ApiResponse.Success("Kyc details deleted successfully", res));
        }

        [HttpGet]
        public async Task<IActionResult> FetchAllKyc(int page = 1, int pageSize = 10, string? verificationStatus = null)
        {
            var res = await repo.FetchAllKyc(page, pageSize, verificationStatus);
            if (res.Items.Count == 0)
            {
                return Ok(ApiResponse.Success("No Kyc found ", res));
            }
            return Ok(ApiResponse.Success("Kyc details Fetched successfully", res));
        }
    }
}
