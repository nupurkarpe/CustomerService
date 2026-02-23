using AutoMapper;
using CustomerService.Application.DTO;
using CustomerService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Application.Mapping
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<CustomerAddDTO, CustomerDetails>().ReverseMap();
            CreateMap<CustomerDetails, CustomerResponseDTO>()
                //.ForMember(x => x.name, x => x.MapFrom(x => x.user.name != null ? x.user.name : "No"))
                //.ForMember(x => x.email, x => x.MapFrom(x => x.user.email != null ? x.user.email : "No"))
                .ReverseMap();
            CreateMap<CustomerUpdateDTO, CustomerDetails>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<DocTypeAddDTO, DocType>().ReverseMap();
            CreateMap<DocType, DocTypeResponseDTO>();

            CreateMap<KycAddDTO, Kyc>()
               .ForMember(dest => dest.filePath, opt => opt.Ignore());
            CreateMap<KycUpdateDTO, Kyc>()
                 .ForMember(dest => dest.customerId, opt => opt.Ignore())
                .ForMember(dest => dest.docTypeId, opt => opt.Ignore())
                 .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Kyc, KycResponseDTO>()
                .ForMember(x => x.typeName, x => x.MapFrom(x => x.docType.typeName != null ? x.docType.typeName : "No"));
        }

    }
}
