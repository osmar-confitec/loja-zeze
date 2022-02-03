using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildBlockCore.Mediator.Messages.Integration
{
  public class UserInsertedIntegrationEvent : IntegrationEvent
    {
        public UserInsertedIntegrationEvent(Guid id,
        string email,
        string name,
        string cPF,
        string publicPlace,
        string number,
        string complement,
        string zipCode,
        string city,
        int typeAddress,
        string district,
        string state)
        {
            Id = id;
            Email = email;
            Name = name;
            CPF = cPF;
            PublicPlace = publicPlace;
            Number = number;
            Complement = complement;
            ZipCode = zipCode;
            City = city;
            TypeAddress = typeAddress;
            District = district;
            State = state;
           
        }

        public Guid Id { get; private set; }
        public string Email { get; private set; }

        public string Name { get; private set; }

        public string CPF { get; private set; }

        public string PublicPlace { get; private set; }
        public string Number { get; private set; }
        public string Complement { get; private set; }
        public string ZipCode { get; private set; }
        public string City { get; private set; }
        public int TypeAddress { get; private set; }
        public string District { get; private set; }
        public string State { get; private set; }

        public string NeighborHood { get; set; }
    }
}
