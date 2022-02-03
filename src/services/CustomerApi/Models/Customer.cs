using BuildBlockCore.DomainObjects;
using BuildBlockCore.Models;
using BuildBlockServices.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApi.Models
{
    public class Customer : EntityDataBase
    {

        public string Name { get; private set; }
        public Email Email { get; private set; }
        public CPF Cpf { get; private set; }
        public virtual Address Address { get; protected set; }


        /*ef constructor*/
        protected Customer() : base()
        {


        }

        public void UpdateCustomer( string name, Email email, CPF cpf, string publicPlace,
                       string number,
                       string complement,
                       string zipCode,
                       string city,
                       TypeAddress typeAddress,
                       string district,
                       string state)
        {


            Name = name;
            Email = email;
            Cpf = cpf;
            Address.UpdateAddress( publicPlace,
                        number,
                        complement,
                        zipCode,
                        city,
                        typeAddress,
                        district,
                        state);

            
            
        }

        public void SetId(Guid id)
        {
            Id = id;
            Address.SetId(id);
            
        }

        public Customer(Guid id,  string name, Email email, CPF cpf, Address address)
        {
            Name = name;
            Email = email;
            Cpf = cpf;
            Address = address;
            Id = id;
        }
    }
}
