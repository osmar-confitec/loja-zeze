using AutoMapper;
using BuildBlockServices.Dto.User;
using BuildBlockServices.Request.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsersApi.Automapper
{
    public class RequestToResponseModelMappingProfile : Profile
    {
        public RequestToResponseModelMappingProfile()
        {
            CreateMap<UserRegisterRequest, UserRegisterDto>();
            CreateMap<UserUpdateRequest, UserUpdateDto>();
            CreateMap<IdentityUser, UserUpdateDto>();
            CreateMap<IdentityUser, UserRegisterDto>();

        }
    }
}
