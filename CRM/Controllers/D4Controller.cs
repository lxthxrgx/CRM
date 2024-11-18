using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRMAgreement.Class;
using SRMAgreement.Data_Base;

namespace SRMAgreement.Controllers
{
    [Route("D4")]
    public class D4Controller : Controller
    {
        private readonly DataBaseContext _context;
        public D4Controller(DataBaseContext context)
        {
            _context = context;
        }

        [HttpPost("DeleteById")]
        public async Task DeleteById(int Id)
        {
            var itemsToDelete = await _context.D4.Where(x => x.Id == Id).ToListAsync();
            _context.D4.RemoveRange(itemsToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
