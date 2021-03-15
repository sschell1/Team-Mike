using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApproval.DAL;
using ProductApproval.Models;
using ProductApproval.Password_and_Authentication_Helpers;
using static ProductApproval.Password_and_Authentication_Helpers.HashProvider;
using Microsoft.Extensions.Configuration;

namespace ProductApproval.Controllers
{
    
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUsersDAO dao;
        private ITokenGenerator tokenGenerator;
        private IPasswordHasher passwordHasher;

        public UsersController(IUsersDAO dataAccessLayer, IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator)
        {
            dao = dataAccessLayer;
            this.tokenGenerator = tokenGenerator;
            this.passwordHasher = passwordHasher;
        }

        [HttpGet]
        public IList<User> GetAllUsers()
        {
            return dao.GetAllUsers();
        }

        [HttpGet("getuser/{username}")]
        public UserEdit GetUser(string username)
        {
            User requestedUser = dao.GetUser(username);
            UserEdit foundUser = new UserEdit();

            foundUser.FirstName = requestedUser.FirstName;
            foundUser.LastName = requestedUser.LastName;
            foundUser.Role = requestedUser.Role;
            foundUser.Username = requestedUser.Username;

            return foundUser;
        }

        [HttpPut("edit/{username}")]
        public ActionResult EditUser(string username, [FromBody]User user)
        {
            if (user.Username != null)
            {
                user = dao.UpdateUser(user);
            }
            return Ok();
        }

        
        [HttpPost("add", Name = "AddUser")]
        public IActionResult AddUser(User user)
        {
            string success = $"{user.Username} successfully added.";
            string failed = $"{user.Username} failed to be added.";
            int response = 0;
            HashedPassword hashedPassword = passwordHasher.HashPassword(user.Password);

            User dbCheck = dao.GetUser(user.Username);
            user.Password = hashedPassword.Password;
            user.Salt = hashedPassword.Salt;
            
            if (dbCheck.Username != user.Username)
            {
                response = dao.AddUser(user);
                var token = tokenGenerator.GenerateToken(user.Username, user.Role);
                return Ok();
            }
            
            return Forbid();
        }

        [HttpDelete("delete/{username}", Name = "DeleteUser")]
        public string DeleteUser(string username)
        {
            string success = $"{username} successfully deleted";
            string failed = $"{username} failed to be removed";
            int response = 0;

            if (username != null)
            {
                response = dao.DeleteUser(username);
            }

            return (response == 1) ? success : failed; ;
        }
    }
}