using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApproval.Password_and_Authentication_Helpers
{
    public interface ITokenGenerator
    {
        string GenerateToken(string username, string role);
    }
}
