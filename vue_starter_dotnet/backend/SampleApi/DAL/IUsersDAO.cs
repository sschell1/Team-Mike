using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductApproval.Models;

namespace ProductApproval.DAL
{
    public interface IUsersDAO
    {
        int AddUser(User user);
        int DeleteUser(string userName);
        User UpdateUser(User user);
        IList<User> GetAllUsers();
        User GetUser(string userName);
    }
}
