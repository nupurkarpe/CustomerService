using CustomerService.Application.DTO;
using CustomerService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Interface
{
    public interface ICustomerRepo
    {
        Task<CustomerDetails> AddCustomer(CustomerAddDTO dto);
        //Task<bool> UserExists(int userId);
        Task<bool> CustomerExists(int customerId);
        Task<CustomerResponseDTO> FetchCustomerById(int customerId);

        Task<CustomerResponseDTO> UpdateCustomer(int customerID, CustomerUpdateDTO dto);

        Task<CustomerResponseDTO> DeleteCustomer(int customerID);

        Task<PagedResult<CustomerResponseDTO>> FetchAllCustomer(int page, int pageSize, string? search);
    }
}
