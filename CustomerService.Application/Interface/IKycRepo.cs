using CustomerService.Application.DTO;
using CustomerService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Interface
{
    public interface IKycRepo
    {
        Task<bool> CustomerExists(int customerId);
        Task<Kyc> AddKyc(KycAddDTO dto);
        Task<KycResponseDTO> GetKycByID(int kycId);
        Task<bool> KycExists(int kycId);
        Task<KycResponseDTO> UpdateKyc(int kycId, KycUpdateDTO dto);
        Task<bool> DocTypeExists(int docTypeId);
        Task<KycResponseDTO> DeleteKyc(int kycId);
        Task<PagedResult<KycResponseDTO>> FetchAllKyc(int page, int pageSize, string? search);
    }
}
