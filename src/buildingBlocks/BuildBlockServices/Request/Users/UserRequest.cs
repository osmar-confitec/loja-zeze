using BuildBlockCore.Models;
using BuildBlockServices.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildBlockServices.Request.Users
{
    public class UserUpdateRequest 
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string Email { get; set; }

       

        [Required(ErrorMessage = "O campo nome é obrigatório")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = " Atenção tamanho do campo nome deve conter até 200 caracteres e minimo de 3 caracteres ")]

        public string Name { get; set; }

        [Required(ErrorMessage = "O campo CPF é obrigatório")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "CPF inválido")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "O logradouro é necessário ")]
        public string PublicPlace { get; set; }

        [Required(ErrorMessage = "O numero é necessário ")]
        public string Number { get; set; }

        [Required(ErrorMessage = "O numero complemento é necessário ")]
        public string Complement { get; set; }
        [Required(ErrorMessage = "O cep é necessário ")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "A cidade é necessária ")]
        public string City { get; set; }

        [Required(ErrorMessage = "O tipo de endereço é necessário ")]
        public TypeAddress TypeAddress { get; set; }

        [Required(ErrorMessage = "O bairro é necessário ")]
        public string District { get; set; }

        [Required(ErrorMessage = "O estado é necessário ")]
        public string State { get; set; }

    }

    public class UserListRequest: PagedDataRequest
    {
        public string Name { get; set; }

    }
    public class UserLoginRequest
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Password { get; set; }
    }
    public class UserRegisterRequest
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "As senhas não conferem.")]
        public string PasswordConfirmation { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = " Atenção tamanho do campo nome deve conter até 200 caracteres e minimo de 3 caracteres ")]

        public string Name { get; set; }

        [Required(ErrorMessage = "O campo CPF é obrigatório")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "CPF inválido")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "O logradouro é necessário ")]
        public string PublicPlace { get;  set; }

        [Required(ErrorMessage = "O numero é necessário ")]
        public string Number { get;  set; }

        [Required(ErrorMessage = "O numero complemento é necessário ")]
        public string Complement { get;  set; }
        [Required(ErrorMessage = "O cep é necessário ")]
        public string ZipCode { get;  set; }

        [Required(ErrorMessage = "A cidade é necessária ")]
        public string City { get;  set; }

        [Required(ErrorMessage = "O tipo de endereço é necessário ")]
        public TypeAddress TypeAddress { get;  set; }

        [Required(ErrorMessage = "O bairro é necessário ")]
        public string District { get;  set; }

        [Required(ErrorMessage = "O estado é necessário ")]
        public string State { get;  set; }
      

    }

}
