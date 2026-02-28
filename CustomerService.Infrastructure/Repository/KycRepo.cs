using AutoMapper;
using CustomerService.Application.DTO;
using CustomerService.Application.Interface;
using CustomerService.Domain.Models;
using CustomerService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Infrastructure.Repository
{
    public class KycRepo : IKycRepo
    {
        ApplicationDbContext db;
        IMapper mapper;
        public KycRepo(ApplicationDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }
        public async Task<Kyc> AddKyc(KycAddDTO dto)
        {
            if (!await CustomerExists(dto.customerId))
            {
                throw new KeyNotFoundException("Customer not found");
            }
            if (dto.file == null || dto.file.Length == 0)
            {
                throw new KeyNotFoundException("Please upload a file");
            }
            var extension = Path.GetExtension(dto.file.FileName).ToLower();
            if (extension != ".pdf")
            {
                throw new InvalidOperationException("Please upload only pdf file");
            }
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "KYC");
            Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}_{dto.file.FileName}";
            var filePath = Path.Combine(folder, fileName);

            var stream = new FileStream(filePath, FileMode.Create);
            await dto.file.CopyToAsync(stream);
            stream.Close();
            var path = $"/Uploads/KYC/{fileName}";
            var kyc = mapper.Map<Kyc>(dto);
            kyc.filePath = path;
            kyc.verificationStatus = "Pending";
            kyc.createdAt = DateTime.UtcNow;
            kyc.docRefNo= Guid.NewGuid().ToString();
            await db.kyc.AddAsync(kyc);
            await db.SaveChangesAsync();
            return kyc;
        }

        public async Task<bool> CustomerExists(int customerId)
        {
            return await db.customerDetails.AnyAsync(x => x.customerId == customerId && x.deletedAt == null);
        }

        public async Task<bool> DocTypeExists(int docTypeId)
        {
            return await db.docType.AnyAsync(x => x.docTypeId == docTypeId && x.deletedAt == null);
        }


        public async Task<KycResponseDTO> GetKycByID(int kycId)
        {
            if (!await KycExists(kycId))
            {
                throw new KeyNotFoundException("Kyc details not found");
            }

            var kyc = await db.kyc.Include(x => x.docType).FirstOrDefaultAsync(x => x.kycId == kycId);
            var res = mapper.Map<KycResponseDTO>(kyc);
            return res;
        }

        public async Task<List<KycResponseDTO>> GetKycByCustomerId(int customerId)
        {
            var kyc = await db.kyc.Include(x => x.docType).Where(x => x.customerId == customerId && x.deletedAt == null).ToListAsync();
            if(kyc == null)
            {
                throw new KeyNotFoundException("Kyc details not found for the customer");
            }
            var res = mapper.Map<List<KycResponseDTO>>(kyc);
            return res;
        }

        public async Task<bool> KycExists(int kycId)
        {
            return await db.kyc.AnyAsync(x => x.kycId == kycId && x.deletedAt == null);
        }

        public async Task<KycResponseDTO> UpdateKyc(int kycId, KycUpdateDTO dto)
        {
            if (!await KycExists(kycId))
            {
                throw new KeyNotFoundException("Kyc details not found");
            }
            var kyc = await db.kyc.Include(x => x.docType).FirstOrDefaultAsync(x => x.kycId == kycId && x.deletedAt == null);
           mapper.Map(dto, kyc);
            if (dto.customerId.HasValue)
            {
                if (!await CustomerExists(dto.customerId.Value))
                {
                    throw new KeyNotFoundException("Customer not found");
                }
                else
                {
                    kyc.customerId = dto.customerId.Value;
                }
            }

            if (dto.docTypeId.HasValue)
            {
                if (!await DocTypeExists(dto.docTypeId.Value))
                {
                    throw new KeyNotFoundException("Doc type not found");
                }
                else
                {
                    kyc.docTypeId = dto.docTypeId.Value;
                }
            }

            if (dto.file != null && dto.file.Length > 0)
            {
                if (dto.file.Length > 250 * 1024)
                {
                    throw new InvalidOperationException("File size should not exceed 250 KB");
                }
                var extension = Path.GetExtension(dto.file.FileName).ToLower();
                if (extension != ".pdf")
                {
                    throw new InvalidOperationException("Please upload only pdf file");
                }
                var existingDocument = await db.kyc.FirstOrDefaultAsync(x =>x.customerId == dto.customerId &&x.docTypeId == dto.docTypeId &&!string.IsNullOrEmpty(x.filePath));
                if (existingDocument != null)
                {
                    throw new InvalidOperationException("Document already submitted for this type");
                }

                var folder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "KYC");
                Directory.CreateDirectory(folder);

                var fileName = $"{Guid.NewGuid()}_{dto.file.FileName}";
                var filePath = Path.Combine(folder, fileName);

                var stream = new FileStream(filePath, FileMode.Create);
                await dto.file.CopyToAsync(stream);
                stream.Close();
                kyc.filePath = $"/Uploads/KYC/{fileName}";
            }
            if (dto.verificationStatus != null)
            {
                kyc.verificationStatus = dto.verificationStatus;
            }

            kyc.modifiedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
            return mapper.Map<KycResponseDTO>(kyc);
        }

        public async Task<KycResponseDTO> DeleteKyc(int kycId)
        {
            if (!await KycExists(kycId))
            {
                throw new KeyNotFoundException("Kyc details not found");
            }
            var kyc = await db.kyc.Include(x => x.docType).FirstOrDefaultAsync(x => x.kycId == kycId && x.deletedAt==null);
            kyc.deletedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
            return mapper.Map<KycResponseDTO>(kyc);
        }
        public async Task<PagedResult<KycResponseDTO>> FetchAllKyc(int page = 1, int pageSize = 10, string? verificationStatus = null)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var kyc = db.kyc.Where(o => o.deletedAt == null).AsQueryable();
            if (!string.IsNullOrEmpty(verificationStatus))
            {
                verificationStatus = verificationStatus.ToLower();
                kyc = kyc.Where(o => o.verificationStatus.ToLower() == verificationStatus);
            }
            var totalItems = await kyc.CountAsync();

            var k = await kyc.Include(x => x.docType).OrderByDescending(o => o.createdAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var result = new PagedResult<KycResponseDTO>
            {
                Items = mapper.Map<List<KycResponseDTO>>(k),
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };
            return result;
        }
    }
}
