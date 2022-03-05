using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApp_UnderTheHood.Pages.Account;

namespace Web_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;
        //dependency injection to get secretKey from configuration
        public AuthController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpPost]
        public IActionResult Authenticate([FromBody]Credential credential)
        {
            //verify credentials
            if (credential.UserName == "admin" && credential.Password == "Password")
            {
                //Creating security context
                // 1) create claims
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@website.com"),
                    new Claim("Department", "HR"),
                    new Claim("Admin", "true"),
                    new Claim("EmploymentDate", "2021-05-01")
                };
                var expiresAt = DateTime.UtcNow.AddMinutes(10);

                return Ok(new
                {
                    access_token = CreateToken(claims, expiresAt),
                    //define expires_at again so consumer of API can use this for validation
                    expires_at = expiresAt
                });
            }
            //return error if unauthorized
            ModelState.AddModelError("Unauthorized", "You're not authorized to access the endpoint.");
            return Unauthorized(ModelState);
        }

        //this method takes in the claims and expiry date and transforms it to a token
        //nugat packages JWT and JWT bearer
        private string CreateToken(IEnumerable<Claim> claims, DateTime expiresAt)
        {
            //in appsettings secretKey define
            var secretKey = Encoding.ASCII.GetBytes(configuration.GetValue<String>("SecretKey"));
            var jwt = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresAt,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature));

            //this returns the token
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
    public class Credential
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
