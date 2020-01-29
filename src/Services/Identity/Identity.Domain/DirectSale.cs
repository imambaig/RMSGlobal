using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Domain
{
    public class DirectSale
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string DirectSaleType { get; set; }

        public DateTime EndDate { get; set; }        

    }
}
