using BuildBlockCore.Mediator.Messages;
using BuildBlockCore.Utils;
using BuildBlockServices.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Application.Commands
{
    public class UpdateCustomerCommand : Command
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Cpf { get; private set; }
        public string PublicPlace { get; private set; }
        public string Number { get; private set; }
        public string Complement { get; private set; }
        public string ZipCode { get; private set; }
        public string City { get; private set; }
        public TypeAddress TypeAddress { get; private set; }
        public string District { get; private set; }
        public string State { get; private set; }

        public UpdateCustomerCommand(LNotifications notifications,
                               Guid _Id,
                               string name,
                               string email,
                               string cpf,
                               string publicPlace,
                               string number,
                               string complement,

                               string zipCode,
                               string city,
                               TypeAddress typeAddress,
                               string district,
                               string state)
      : base(notifications)
        {
            Id = _Id;
            Name = name;
            Email = email;
            Cpf = cpf;
            PublicPlace = publicPlace;
            Number = number;
            Complement = complement;
            ZipCode = zipCode;
            City = city;
            TypeAddress = typeAddress;
            District = district;
            State = state;
        }

    }
}
