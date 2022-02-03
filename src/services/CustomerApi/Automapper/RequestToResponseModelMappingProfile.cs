using AutoMapper;
using CustomerApi.Application.Commands;
using CustomerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Automapper
{
    public class RequestToResponseModelMappingProfile : Profile
    {
        public RequestToResponseModelMappingProfile()
        {
            CreateMap< InsertCustomerCommand,Customer>
                ()
                
                .ConstructUsing(x=> 

                     new Customer(
                                        x.Id,    
                                        x.Name,
                                         new BuildBlockCore.DomainObjects.Email(x.Email),
                                         new BuildBlockCore.DomainObjects.CPF(x.Cpf),
                                         new Address(
                                               x.PublicPlace,
                                                x.Number,
                                                x.Complement,
                                                x.ZipCode, 
                                                x.City, 
                                                x.TypeAddress,
                                                x.District,
                                                x.State
                                            )
                                        )).ForAllOtherMembers(opts => opts.Ignore());
        }
    }
}
