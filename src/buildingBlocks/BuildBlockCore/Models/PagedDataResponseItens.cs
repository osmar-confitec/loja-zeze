using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildBlockCore.Models
{
   public class PagedDataResponseItens
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalItens { get; set; }

        public int TotalPages { get; set; }
    }
}
