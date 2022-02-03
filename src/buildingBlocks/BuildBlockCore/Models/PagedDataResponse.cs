using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildBlockCore.Models
{
    public class PagedDataResponse<T> : PagedDataResponseItens where T : class
    {

        public List<T> Items { get; set; }

      

        public PagedDataResponse()
        {
            Items = new List<T>();
        }

    }
}
