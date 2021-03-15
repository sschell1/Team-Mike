using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProductApproval.Password_and_Authentication_Helpers.HashProvider;

namespace ProductApproval.Password_and_Authentication_Helpers
{
    public interface IPasswordHasher
    {
        HashedPassword HashPassword(string plainTextPassword);

        bool VerifyPasswordMatch(string existingHashedPassword, string plainTextPassword, string salt);
    }
}
