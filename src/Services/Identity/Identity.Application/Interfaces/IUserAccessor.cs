using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.Interfaces
{
    public interface IUserAccessor
    {
        string GetCurrentUsername();
    }
}
