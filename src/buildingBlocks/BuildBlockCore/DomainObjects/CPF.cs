using BuildBlockCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildBlockCore.DomainObjects
{
    public class CPF
    {
        public const int CpfMaxLength = 11;
        public string Number { get; private set; }


        /**/
        protected CPF()
        {

        }

        public CPF(string number)
        {

            if (!number.IsCpf())
            {
                throw new DomainException("CPF Inválido");
            }
            Number = number.OnlyNumbers();
        }


    }
}
