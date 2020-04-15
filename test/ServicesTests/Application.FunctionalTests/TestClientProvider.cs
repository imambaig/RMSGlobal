using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.FunctionalTests
{
    public class TestClientProvider
    {
        public TestClientProvider()
        {
            Version servevr = new TestServer();
        }
    }
}
