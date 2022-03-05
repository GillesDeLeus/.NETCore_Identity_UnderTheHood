using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_App.Data.Account
{
    public class User: IdentityUser
    {
        public string Department { get; set; }
        public string Position { get; set; }
    }
}
