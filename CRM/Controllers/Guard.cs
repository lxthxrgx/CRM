using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRMAgreement.Data_Base;
using System.Security.Cryptography;

using SRMAgreement.Class;

namespace SRMAgreement.Controllers
{
    [Route("[controller]/[action]")]
    public class Guard : Controller
    {
        private readonly DataBaseContext _context;

        public Guard(DataBaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDataModalWindowRespPerson(int NumberGroup)
        {
            var GetDataFromDbBynumber = _context.D5.FirstOrDefault(x => x.NumberGroup == NumberGroup);

            if(GetDataFromDbBynumber == null)
            {
                var D5AddRespPerson_Phone = new _5D
                {
                    NumberGroup = NumberGroup,
                    NameGroup = string.Empty,
                    address = string.Empty,
                    OhronnaComp = string.Empty,
                    NumDog = string.Empty,
                    NumDog2 = string.Empty,
                    StrokDii = DateTime.MinValue,
                    StrokDii2 = DateTime.MinValue,
                    ResPerson = string.Empty,   
                    Phone = string.Empty
                };

            }

            return Json(new
            {
                success = true,
                data = new
                {
                    ResPerson = GetDataFromDbBynumber.ResPerson,
                    Phone = GetDataFromDbBynumber.Phone
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> AdddDataToDatabaseGuard(int NumberGroup, string RespPerson, string Phone)
        {
            Console.WriteLine($"Received NumberGroup: {NumberGroup}");
            var GetDataFromDbBynumber = await _context.D5.FirstOrDefaultAsync(x => x.NumberGroup == NumberGroup);

            if (GetDataFromDbBynumber == null)
            {
                Console.WriteLine($"Record not found for NumberGroup: {NumberGroup}");
                return NotFound(new { status = false, message = "Record not found for the given NumberGroup." });
            }

            GetDataFromDbBynumber.ResPerson = RespPerson;
            GetDataFromDbBynumber.Phone = Phone;

            await _context.SaveChangesAsync();

            return Ok(new { status = true, message = "Data saved in DataBase" });
        }
    }
}
