using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SRMAgreement.Class;
using SRMAgreement.Data_Base;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using SRMAgreement.SuppCode;
using System.Xml.Linq;
using Windows.System;
using User = SRMAgreement.Class.User;

namespace SRMAgreement.Controllers
{
    [Route("RegistrationController")]
    public class RegistrationController : Controller
    {
        private readonly DataBaseUser _user;

        public RegistrationController(DataBaseUser user)
        {
            _user = user;
        }
        [Route("Registraion")]
        public async Task<IActionResult> Registraion(string Name, string Username, string Password)
        {
            // Assign default role
            var defaultRole = "User";

            var newUser = new User
            {
                Name = Name,
                Username = Username,
                Password = HashHelper.ComputeSha512Hash(Password),
                Role = defaultRole // Set default role
            };

            _user.User.Add(newUser);
            await _user.SaveChangesAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, newUser.Username),
                new Claim(ClaimTypes.NameIdentifier, newUser.Username),
                new Claim("nickname", newUser.Name),
                new Claim(ClaimTypes.Role, newUser.Role)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(10),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Ok(claimsIdentity);
        }
    }
}