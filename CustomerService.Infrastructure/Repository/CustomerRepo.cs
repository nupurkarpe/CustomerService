using AutoMapper;
using Azure;
using CustomerService.Application.DTO;
using CustomerService.Application.Interface;
using CustomerService.Domain.Models;
using CustomerService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace CustomerService.Infrastructure.Repository
{
    public class CustomerRepo : ICustomerRepo
    {
        ApplicationDbContext db;
        IMapper mapper;
        UserServiceClient client;
        public CustomerRepo(ApplicationDbContext db, IMapper mapper, UserServiceClient client)
        {
            this.db = db;
            this.mapper = mapper;
            this.client = client;
        }
       
        public async Task<CustomerDetails> AddCustomer(CustomerAddDTO dto)
        {
            var userExists = await client.GetUserById(dto.userId);

            if (userExists==null)
            {
                throw new KeyNotFoundException("User Not Found");
            }
            var customerExists = await db.customerDetails.AnyAsync(c => c.createdBy == dto.userId);
            if (customerExists)
            {
                throw new InvalidOperationException("Customer already exists for this user ID.");
            }
            var data = mapper.Map<CustomerDetails>(dto);
            data.createdBy = dto.userId;
            data.status = "Pending";
            await db.AddAsync(data);
            await db.SaveChangesAsync();
            return data;
        }
        public async Task<CustomerResponseDTO>CustExists(int userId)
        {
            var customerExists = await db.customerDetails.FirstOrDefaultAsync(c => c.createdBy == userId);

            return mapper.Map<CustomerResponseDTO>(customerExists);
        }
        public async Task<CustomerResponseDTO> FetchCustomerById(int customerId)
        {
            var cus = await db.customerDetails.FirstOrDefaultAsync(c => c.customerId == customerId && c.deletedAt == null);
            if (cus == null)
                throw new KeyNotFoundException("Customer not found");
            
            var res = mapper.Map<CustomerResponseDTO>(cus);
            var user = await client.GetUserById(cus.userId);

            if (user != null)
            {
                res.name = user.name;
                res.email = user.email;
            }

            return res;
        }

        public async Task<bool> CustomerExists(int customerId)
        {
            return await db.customerDetails.AnyAsync(x => x.customerId == customerId);
        }

        public async Task<CustomerResponseDTO> UpdateCustomer(int customerID, CustomerUpdateDTO dto)
        {
            var cus = await db.customerDetails.FirstOrDefaultAsync(c => c.customerId == customerID && c.deletedAt == null);
            if (cus == null)
                throw new KeyNotFoundException("Customer not found");
            var data = mapper.Map(dto, cus);
            data.modifiedBy = data.userId;
            data.modifiedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
            var res = mapper.Map<CustomerResponseDTO>(cus);
            var user = await client.GetUserById(cus.userId);
            if (user != null)
            {
                res.name = user.name;
                res.email = user.email;
            }
            return res;
        }

        public async Task<CustomerResponseDTO> DeleteCustomer(int customerID)
        {          
            var cus = await db.customerDetails.FirstOrDefaultAsync(c => c.customerId == customerID && c.deletedAt == null);
            if (cus == null)
                throw new KeyNotFoundException("Customer not found");
            cus.deletedBy = customerID;
            cus.deletedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
            var res = mapper.Map<CustomerResponseDTO>(cus);
            var user = await client.GetUserById(cus.userId);
            if (user != null)
            {
                res.name = user.name;
                res.email = user.email;
            }
            return res;
        }

        public async Task<PagedResult<CustomerResponseDTO>> FetchAllCustomer(int page, int pageSize, string? name)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var cus = db.customerDetails.Where(o => o.deletedAt == null).AsQueryable();
            var users = await client.GetAllUser();

           
            if (!string.IsNullOrEmpty(name))
            {
                name = name.ToLower();
                var userIds = users
                    .Where(o => o.name.ToLower().Contains(name))
                    .Select(u => u.userId)
                    .ToList();

                cus = cus.Where(c => userIds.Contains(c.userId));
            }
            var totalItems = await cus.CountAsync();

            var cust = await cus.OrderByDescending(o => o.createdAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
         

            var responseList = cust.Select(c =>
            {
                var response = mapper.Map<CustomerResponseDTO>(c);
                var user = users.FirstOrDefault(u => u.userId == c.userId);
                if (user != null)
                {
                    response.name = user.name;
                    response.email = user.email;
                }
                return response;
            }).ToList();

           
            var result = new PagedResult<CustomerResponseDTO>
            {
                Items = responseList,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };
            return result;
        }
    }
}
