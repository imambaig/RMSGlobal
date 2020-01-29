using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Domain
{
    public class AppUser :IdentityUser
    {
        public string DisplayName { get; set; }
    }
}
