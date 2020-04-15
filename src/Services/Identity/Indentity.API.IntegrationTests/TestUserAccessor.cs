using Identity.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Indentity.API.IntegrationTests
{
    public class TestUserAccessor : IUserAccessor
    {
        public string GetCurrentUsername()
        {
            return "imam@test.com";
        }
    }
}
