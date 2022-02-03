using BuildBlockCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildBlockCore.DomainObjects
{
    public class Email
    {
        public const int EmailMaxLength = 254;
        public const int EmailMinLength = 5;
        public string Mail { get; private set; }

        //Construtor do EntityFramework
        protected Email() { }

        public Email(string mail)
        {
            if (!mail.IsValidEmail()) throw new DomainException("E-mail inválido");
            Mail = mail;
        }

    }
}
