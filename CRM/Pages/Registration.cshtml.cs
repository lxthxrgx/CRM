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

namespace SRMAgreement.Pages
{
    public class RegistrationModel : PageModel
    {
        private readonly DataBaseUser _user;
        //private readonly DataBaseContext _context;

        [BindProperty]
        [Required]
        public string Name { get; set; }

        [BindProperty]
        [Required]
        public string Username { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public RegistrationModel(/*DataBaseContext context*/DataBaseUser user)
        {
            //_context = context;
            _user = user;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

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

            return RedirectToPage("Index");
        }
    }
}
