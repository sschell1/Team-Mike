using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApproval.DAL;
using ProductApproval.Models;
using ProductApproval.Password_and_Authentication_Helpers;

namespace ProductApproval.Controllers
{
    
    [Route("api/login")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IUsersDAO userDAO;
        private IPasswordHasher passwordHasher;
        private ITokenGenerator tokenGenerator;

        public AccountController(ITokenGenerator tokenGenerator, IUsersDAO userDao, IPasswordHasher passwordHasher)
        {
            this.tokenGenerator = tokenGenerator;
            this.passwordHasher = passwordHasher;
            this.userDAO = userDao;
        }

        [HttpPost]
        public IActionResult Login(AuthenticateUser model)
        {
            IActionResult result = Unauthorized();

            var user = userDAO.GetUser(model.Username);

            if (user.Username != null && passwordHasher.VerifyPasswordMatch(user.Password, model.Password, user.Salt))
            {
                var token = tokenGenerator.GenerateToken(user.Username, user.Role);
                result = Ok(token);
            }
            return result;
        }
    }

    [Route("values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var result = $"Welcome back {User.Identity.Name}";
            return Ok(result);
        }

        [HttpGet("user")]
        [Authorize(Roles = "User")]
        public IActionResult RequestToken()
        {
            var result = "If you see this then you are a user.";
            return Ok(result);
        }
    }
}
