using CustomerService.Application.DTO;
using CustomerService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Interface
{
    public interface IDocTypeRepo
    {
        Task<bool> DocTypeNameExists(string typeName);
        Task<DocType> AddDocType(DocTypeAddDTO dto);
        Task<bool> DocTypeExists(int docTypeId);
        Task<DocTypeResponseDTO> FetchDocTypeById(int docTypeId);
        Task<DocTypeResponseDTO> UpdateDocType(int docTypeId, DocTypeAddDTO dto);
        Task<DocTypeResponseDTO> DeleteDocType(int docTypeId);
        Task<PagedResult<DocTypeResponseDTO>> FetchAllDocType(int page = 1, int pageSize = 10, string? typeName = null);
    }
}
