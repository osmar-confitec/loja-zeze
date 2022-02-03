using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildBlockCore.Models
{
    public class PagedDataRequest
    {
        public int Page { get; set; }

        public int Limit { get; set; }

        protected PagedDataRequest()
        {

            Limit = 30;

            Page = 1;

        }
    }
}
