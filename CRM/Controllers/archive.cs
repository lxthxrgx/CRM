using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRMAgreement.Class;
using SRMAgreement.Data_Base;
using System.Linq;
using System.Threading.Tasks;

namespace SRMAgreement.Controllers
{
    [Route("archive")]
    public class ArchiveController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly DataBaseArchive _contextarchive;
        public ArchiveController(DataBaseContext context, DataBaseArchive contextarchive)
        {
            _context = context;
            _contextarchive = contextarchive;
        }

        [HttpPost("AddToArchive")]
        public async Task OnPostAddToArchive(int Id)
        {
            var selectedDataToArchive = await _context.D4
                .Where(x => x.Id == Id)
                .Select(x => new Archive_4D
                {
                    NumberGroup = x.NumberGroup,
                    NameGroup = x.NameGroup,
                    address = x.address,
                    DogovirSuborendu = x.DogovirSuborendu,
                    DateTime = x.DateTime,
                    Suma = x.Suma,
                    AktDate = x.AktDate
                })
                .ToListAsync();

            _contextarchive.Archive_4D.AddRange(selectedDataToArchive);
            await _contextarchive.SaveChangesAsync();

            var itemsToDelete = await _context.D4.Where(x => x.Id == Id).ToListAsync();
            _context.D4.RemoveRange(itemsToDelete);
            await _context.SaveChangesAsync();
        }

        [HttpPost("AddToMainDB")]
        public async Task OnPostAddToMainDB(int Id)
        {
            var selectedDataToMain = await _contextarchive.Archive_4D
                .Where(x => x.Id == Id)
                .Select(x => new _4D
                {
                    NumberGroup = x.NumberGroup,
                    NameGroup = x.NameGroup,
                    address = x.address,
                    DogovirSuborendu = x.DogovirSuborendu,
                    DateTime = x.DateTime,
                    Suma = x.Suma.ToString(),
                    AktDate = x.AktDate
                })
                .ToListAsync();

            _context.D4.AddRange(selectedDataToMain);
            await _context.SaveChangesAsync();
        }
    }
}
