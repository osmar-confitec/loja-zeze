using BuildBlockCore.Models;
using BuildBlockServices.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Models
{
  
    public class Address : EntityDataBase
    {
        public string PublicPlace { get; private set; }
        public string Number { get; private set; }
        public string Complement { get; private set; }
        public string ZipCode { get; private set; }
        public string City { get; private set; }
        public TypeAddress TypeAddress { get; private set; }
        public string District { get; private set; }
        public string State { get; private set; }
        public virtual Customer Customer { get; protected set; }

        protected Address() : base()
        {
            
        }

        public void SetId(Guid id)
        {
            Id = id;

        }

        public void UpdateAddress(string publicPlace,
                       string number,
                       string complement,
                       string zipCode,
                       string city,
                       TypeAddress typeAddress,
                       string district,
                       string state
                       )
        {
            PublicPlace = publicPlace;
            Number = number;
            Complement = complement;
            ZipCode = zipCode;
            City = city;
            TypeAddress = typeAddress;
            District = district;
            State = state;
        }

        public Address(string publicPlace,
                       string number,
                       string complement,
                       string zipCode,
                       string city,
                       TypeAddress typeAddress,
                       string district,
                       string state
                       )
        {
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
