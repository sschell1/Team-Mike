﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApproval.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Role { get; set; }
        public string Salt { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}
