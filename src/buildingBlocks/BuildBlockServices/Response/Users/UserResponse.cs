using BuildBlockCore.Models;
using BuildBlockServices.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildBlockServices.Response.Users
{
    public class UserDeleteResponse : BaseResponseApi<UserDeleteDto> { }
    public class UserLoginResponse : BaseResponseApi<UserLoginDto> { }
    public class UserRegisterResponse : BaseResponseApi<UserRegisterDto> { }
    public class UserUpdateResponse : BaseResponseApi<UserUpdateDto> { }

    public class UserListResponse : BaseResponseApi<PagedDataResponse<UserListDto>> { }

    public class UserItemResponse : BaseResponseApi<UserUpdateDto> { }

    public class CustomerRegisterResponse:BaseResponseApi<CustomerRegisterDto> { }
    public class CustomerUpdateResponse:BaseResponseApi<CustomerUpdateDto> { }


}
