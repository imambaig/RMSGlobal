using System;
using System.Collections.Generic;
using System.Text;

namespace Seller.Domain.Exceptions
{
    public class SalesSessionDomainException : Exception
    {
        public SalesSessionDomainException()
        { }

        public SalesSessionDomainException(string message)
            : base(message)
        { }

        public SalesSessionDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
