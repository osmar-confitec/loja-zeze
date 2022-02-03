using BuildBlockCore.Data.Interfaces;
using BuildBlockServices.Dto.User;
using BuildBlockServices.Request.Users;
using BuildBlockServices.Response.Users;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Application.Queries
{
    public interface ICustomerQueries
    {

        Task<IEnumerable<UserListDto>> GetUsersList(UserListRequest userListRequest);

    }

    public class CustomerQueries : ICustomerQueries
    {
        readonly IUnitOfWork _unitOfWork;
        public CustomerQueries(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<UserListDto>> GetUsersList(UserListRequest userListRequest)
        {
            string sql = @"
                                   SELECT [Id]
                                      ,[Name]
                                      ,[Email]
                                      ,[Cpf]
                                      ,[Active]
                                      ,[DateRegister]
                                      ,[DateUpdate]
                                      ,[UserInsertedId]
                                      ,[UserUpdatedId]
                                      ,[DeleteDate]
                                      ,[UserDeletedId]
                                  FROM [dbo].[Customers]
                                  WHERE 1=1
                                ";

            if (!string.IsNullOrEmpty(userListRequest.Name))
                sql += " AND  [dbo].[Customers] LIKE @Name ";


            return await _unitOfWork.GetContext()
                               .Database
                               .GetDbConnection()
                               .QueryAsync<UserListDto>(sql: sql,
                                                        param: new { Name = $"%{userListRequest.Name}%" }
                                                        );

        }
    }
}
