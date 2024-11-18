using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using SRMAgreement.Class;
using SRMAgreement.Data_Base;

namespace SRMAgreement.Controllers
{
    [Route("AccountingCon")]
    public class AccountingCon : Controller
    {
        private readonly DataBaseContext _context;

        public AccountingCon(DataBaseContext context)
        {
            _context = context;
        }

        [HttpGet("GetGroupByNameGroupAsync")]
        public async Task<IActionResult> GetGroupByNameGroupAsync()
        {
            List<string> nameGroups = await _context.D2
                .Select(x => x.NameGroup)
                .Distinct()
                .ToListAsync();

            return Ok(nameGroups);
        }
    }
}
