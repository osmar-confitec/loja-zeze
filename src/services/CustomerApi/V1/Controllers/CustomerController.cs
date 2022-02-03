using BuildBlockCore.Data;
using BuildBlockCore.Identity;
using BuildBlockCore.Mediator;
using BuildBlockCore.Models;
using BuildBlockCore.Utils;
using BuildBlockMessageBus.MessageBus;
using BuildBlockServices.Controllers;
using BuildBlockServices.Dto.User;
using BuildBlockServices.Request.Users;
using BuildBlockServices.Response.Users;
using CustomerApi.Application.Commands;
using CustomerApi.Application.Queries;
using CustomerApi.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.V1.Controllers
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/customer")]
    [Authorize]
    public class CustomerController : MainController
    {

        readonly ICustomerRepository _customerRepository;
        readonly IMediatorHandler _mediatorHandler;
       


        public CustomerController(IMediatorHandler mediatorHandler,
            
                                 LNotifications notifications, ICustomerRepository customerRepository)
                    : base(notifications)
        {

            _mediatorHandler = mediatorHandler;
            _customerRepository = customerRepository;
            

        }

        [HttpGet("obter-customer-id/{id}")]
        [ClaimsAuthorize("UsersAdm", "1")]
        public async Task<IActionResult> GetCustomerById([FromRoute] string id)
        {
            return await ExecControllerAsync(async () =>
            {
                var customerSearch = (await _customerRepository._repositoryConsult.SearchAsync(x => x.Id.ToString() == id));
                if (customerSearch != null && customerSearch.Any())
                {
                    var customer = customerSearch.FirstOrDefault();
                    var address = customer.Address;
                    return new UserUpdateDto
                    {

                        Active = customer.Active,
                        Cpf = customer.Cpf.Number.FormatCPF(),
                        Email = customer.Email.Mail,
                        Name = customer.Name,
                        City = address.City,
                        Complement = address.Complement,
                        District = address.District,
                        Id = customer.Id,
                        Number = address.Number,
                        PublicPlace = address.PublicPlace,
                        State = address.State,
                        TypeAddress = address.TypeAddress,
                        ZipCode = address.ZipCode
                    };
                }
                else
                    return null;
            });
        }

        [HttpPut("atualizar-customer")]
        [ClaimsAuthorize("UsersAdm", "1")]
        public async Task<IActionResult> UpdateCustomer([FromBody] UserUpdateRequest userUpdateRequest)
        {

            if (!ModelState.IsValid) return ReturnModelState(ModelState);

            return await ExecControllerAsync(async () =>
            {

                var updateCustomer = new UpdateCustomerCommand(notifications: _notifications,
                                                               _Id: userUpdateRequest.Id,
                                                               name: userUpdateRequest.Name,
                                                               email: userUpdateRequest.Email,
                                                               cpf: userUpdateRequest.CPF,
                                                               publicPlace: userUpdateRequest.PublicPlace,
                                                               number: userUpdateRequest.Number,
                                                               complement: userUpdateRequest.Number,
                                                               zipCode: userUpdateRequest.ZipCode,
                                                               city: userUpdateRequest.City,
                                                               typeAddress: userUpdateRequest.TypeAddress,
                                                               district: userUpdateRequest.District,
                                                               state: userUpdateRequest.State);
                var returnCommand = await _mediatorHandler.SendCommand(updateCustomer);
                if (returnCommand != null && returnCommand.Any())
                    return null;
                else
                {
                    var customerSearc = await _customerRepository._repositoryConsult.SearchAsync(x => x.Id == userUpdateRequest.Id);
                    return new CustomerUpdateDto
                    {


                    };

                }

            });

        }


        [HttpDelete("deletar-customer/{id}")]
        [ClaimsAuthorize("UsersAdm", "1")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] Guid id)
        {
            return await ExecControllerAsync(async () =>
            {

                var customerDelete = new DeleteCustomerCommand(_notifications, id);
                var returnCommand = await _mediatorHandler.SendCommand(customerDelete);
                if (returnCommand != null && returnCommand.Any())
                    return null;
                else
                {
                    return new UserDeleteDto
                    {


                    };
                }
            });
        }

        [HttpGet("obter-customer")]
        [ClaimsAuthorize("UsersAdm", "1")]
        public async Task<IActionResult> GetCustomer([FromQuery] UserListRequest userListRequest)
        {
            return await ExecControllerAsync(async () =>
            {
                var query = _customerRepository._repositoryConsult.GetQueryable();
                var resp = new PagedDataResponse<UserListDto>();

                if (!string.IsNullOrEmpty(userListRequest.Name))
                    query = query.Where(x => x.Name.Contains(userListRequest.Name));


                var results = await query.PaginateAsync(userListRequest);

                resp.Page = results.Page;
                resp.PageSize = results.PageSize;
                resp.TotalItens = results.TotalItens;
                resp.TotalPages = results.TotalPages;

                resp.Items = results.Items.Select(x =>
                        new UserListDto
                        {
                            Active = x.Active,
                            Cpf = x.Cpf.Number.FormatCPF(),
                            Email = x.Email.Mail,
                            Name = x.Name,
                            Id = x.Id
                        }).ToList();

                return resp;
            });
        }
    }
}
