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
    public class DocTypeRepo : IDocTypeRepo
    {
        ApplicationDbContext db;
        IMapper mapper;
        public DocTypeRepo(ApplicationDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<bool> DocTypeNameExists(string typeName)
        {
            return await db.docType.AnyAsync(c => c.typeName.ToLower() == typeName.ToLower());
        }
        public async Task<DocType> AddDocType(DocTypeAddDTO dto)
        {
            if (await DocTypeNameExists(dto.typeName))
            {
                throw new InvalidOperationException("Doc Type already exists");
            }
            var data = mapper.Map<DocType>(dto);
            await db.AddAsync(data);
            await db.SaveChangesAsync();
            return data;
        }

        public async Task<bool> DocTypeExists(int docTypeId)
        {
            return await db.docType.AnyAsync(x => x.docTypeId == docTypeId);
        }

        public async Task<DocTypeResponseDTO> FetchDocTypeById(int docTypeId)
        {
            if (!await DocTypeExists(docTypeId))
            {
                throw new KeyNotFoundException("Doc Type not found");
            }
            var doc = await db.docType.FirstOrDefaultAsync(d => d.docTypeId == docTypeId);
            var res = mapper.Map<DocTypeResponseDTO>(doc);
            return res;
        }

        public async Task<DocTypeResponseDTO> UpdateDocType(int docTypeId, DocTypeAddDTO dto)
        {
            if (!await DocTypeExists(docTypeId))
            {
                throw new KeyNotFoundException("Doc Type not found");
            }
            if (!String.IsNullOrEmpty(dto.typeName))
            {
                if (await DocTypeNameExists(dto.typeName))
                {
                    throw new InvalidOperationException("Doc Type already exists");
                }
            }
            var doc = await db.docType.FirstOrDefaultAsync(c => c.docTypeId == docTypeId);
            var data = mapper.Map(dto, doc);
            data.modifiedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
            return mapper.Map<DocTypeResponseDTO>(doc);
        }

        public async Task<DocTypeResponseDTO> DeleteDocType(int docTypeId)
        {
            if (!await DocTypeExists(docTypeId))
            {
                throw new KeyNotFoundException("Doc Type not found");
            }
            var doc = await db.docType.FirstOrDefaultAsync(c => c.docTypeId == docTypeId);
            doc.deletedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
            return mapper.Map<DocTypeResponseDTO>(doc);
        }

        public async Task<PagedResult<DocTypeResponseDTO>> FetchAllDocType(int page = 1, int pageSize = 10, string? typeName = null)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var docT = db.docType.Where(o => o.deletedAt == null).AsQueryable();

            if (!string.IsNullOrEmpty(typeName))
            {
                typeName = typeName.ToLower();
                docT = docT.Where(o => o.typeName.ToLower() == typeName);
            }
            var totalItems = await docT.CountAsync();

            var d = await docT.OrderByDescending(o => o.createdAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var result = new PagedResult<DocTypeResponseDTO>
            {
                Items = mapper.Map<List<DocTypeResponseDTO>>(d),
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };
            return result;
        }
    }
}
