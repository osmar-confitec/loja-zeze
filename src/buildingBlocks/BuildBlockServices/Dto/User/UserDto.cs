using BuildBlockServices.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildBlockServices.Dto.User
{

    public class CustomerUpdateDto
    { 
    }

    public class CustomerRegisterDto { 
    
    }

    public class UserDeleteDto
    { 
    
    }
    public class UserUpdateDto
    {
        public string Name { get; set; }

        public string Cpf { get; set; }

        public bool Active { get; set; }

        public string Email { get; set; }

        public Guid Id { get; set; }

        public string PublicPlace { get;  set; }
        public string Number { get;  set; }
        public string Complement { get;  set; }
        public string ZipCode { get;  set; }
        public string City { get;  set; }
        public TypeAddress TypeAddress { get;  set; }
        public string District { get;  set; }
        public string State { get;  set; }
    }

    public class UserListDto
    {

        public string Name { get; set; }

        public string Cpf { get; set; }

        public bool Active { get; set; }

        public string Email { get; set; }

        public Guid Id { get; set; }

    }


    public class UserTokenDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<UserClaimDto> Claims { get; set; }

        public UserTokenDto()
        {
            Claims = new List<UserClaimDto>();
        }
    }

    public class UserRegisterDto
    {
        public Guid Id { get; set; }
    }
    public class UserClaimDto
    {
        public string Value { get; set; }
        public string Type { get; set; }
    }

    public class UserLoginDto
    {
        public string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public UserTokenDto UserToken { get; set; }
    }


}
