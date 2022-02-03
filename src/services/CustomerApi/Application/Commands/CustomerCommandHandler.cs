using AutoMapper;
using BuildBlockCore.Data.Interfaces;
using BuildBlockCore.Identity;
using BuildBlockCore.Mediator.Messages;
using BuildBlockCore.Mediator.Messages.Integration;
using BuildBlockCore.Utils;
using BuildBlockMessageBus.MessageBus;
using CustomerApi.Data.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerApi.Application.Commands
{
    public class CustomerCommandHandler : CommandHandler,
        IRequestHandler<InsertCustomerCommand, LNotifications>,
        IRequestHandler<UpdateCustomerCommand, LNotifications>,
        IRequestHandler<DeleteCustomerCommand, LNotifications>
    
    {


        readonly IMapper _mapper;
        readonly ICustomerRepository _customerRepository;
        readonly IUser _user;

        readonly IMessageBus _messageBus;



        public CustomerCommandHandler(ICustomerRepository customerRepository,
                                      LNotifications notifications,
                                      IUnitOfWork unitOfWork,
                                      IUser user,
                                      IMessageBus messageBus, 
                                      IMapper mapper)
               : base(notifications, unitOfWork)
        {
            //
            _mapper = mapper;
            _user = user;
            _messageBus = messageBus;
            _customerRepository = customerRepository;
        }

        public async Task<LNotifications> Handle(InsertCustomerCommand request, CancellationToken cancellationToken)
        {
            var customerCPF = (await _customerRepository._repositoryConsult.SearchAsync(x => x.Cpf.Number == request.Cpf.OnlyNumbers()))?.FirstOrDefault();
            if (customerCPF != null)
            {
                var noty = new LNotifications();
                noty.Add(new LNotification { Message = $"Atenção CPF já existente para o Cliente {customerCPF.Name} com o status {customerCPF.Active} " });
                return noty;
            }

            var customerSave = _mapper.Map<Models.Customer>(request);
            customerSave.SetId(request.Id);
            customerSave.UserInsertedId = _user.GetUserAdm();
            customerSave.DateRegister = DateTime.Now;
            await _customerRepository.AddAsync(customerSave);
            await _unitOfWork.CommitAsync();
            return new LNotifications();
        }

        public async Task<LNotifications> Handle(UpdateCustomerCommand request,
                                                 CancellationToken cancellationToken)
        {
            try
            {
                var customerCPF = (await _customerRepository._repositoryConsult.SearchAsync(x => x.Id != request.Id
                                                                                    && x.Cpf.Number == request.Cpf.OnlyNumbers()))
                                                                                    ?.FirstOrDefault();
                if (customerCPF != null)
                {
                    var noty = new LNotifications();
                    noty.Add(new LNotification { Message = $"Atenção CPF já existente para o Cliente {customerCPF.Name} com o status {customerCPF.Active} " });
                    return noty;
                }

                var customerSearch = (await _customerRepository._repositoryConsult
                                                                .SearchAsync(x => x.Id == request.Id))?
                                                                .FirstOrDefault();

                if (customerSearch != null)
                {

                    customerSearch.UpdateCustomer(request.Name, new BuildBlockCore.DomainObjects.Email(request.Email),
                        new BuildBlockCore.DomainObjects.CPF(request.Cpf.OnlyNumbers()),
                   number: request.Number,
                                           publicPlace: request.PublicPlace,
                                           complement: request.Complement,
                                           zipCode: request.ZipCode,
                                           city: request.City,
                                           typeAddress: request.TypeAddress,
                                           district: request.District,
                                           state: request.State);
                    customerSearch.DateUpdate = DateTime.Now;
                    customerSearch.UserUpdatedId = _user.GetUserAdm();
                }
                await _unitOfWork.CommitAsync();
                return new LNotifications();

            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public async Task<LNotifications> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customerSearch = (await _customerRepository._repositoryConsult
                                                              .SearchAsync(x => x.Id == request.Id))?
                                                              .FirstOrDefault();

            customerSearch.Active = false;
            customerSearch.DeleteDate = DateTime.Now;
            customerSearch.UserDeletedId = _user.GetUserAdm();
            await _unitOfWork.CommitAsync();
            var respMessage = await _messageBus.RequestAsync<UserDeletedIntegrationEvent, ResponseMessage>(new
                                  UserDeletedIntegrationEvent(
                                   request.Id
                            ));
            if (respMessage.Notifications.Any())
            {
                _notifications.AddRange(respMessage.Notifications);
                customerSearch.Active = true;
                customerSearch.DeleteDate = null;
                customerSearch.UserDeletedId = null;
                await _unitOfWork.CommitAsync();
            }

            return _notifications;
        }
    }
}
