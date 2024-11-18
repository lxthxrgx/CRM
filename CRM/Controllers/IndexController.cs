using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRMAgreement.Data_Base;

namespace SRMAgreement.Controllers
{
    [Route("IndexController")]
    public class IndexController : Controller
    {

        private readonly DataBaseContext _context;
        public IndexController(DataBaseContext context)
        {
            _context = context;
        }

        public class CheckboxUpdateModel
        {
            public int Id { get; set; }
            public bool IsDone { get; set; }
        }

        [HttpPost("UpdateDone")]
        public IActionResult UpdateDone([FromBody] CheckboxUpdateModel model)
        {

            var record =  _context.D4.FirstOrDefault(r => r.Id == model.Id);
            if (record == null)
            {
                return NotFound("Record not found.");
            }

            record.Done = model.IsDone;

            _context.SaveChangesAsync();

            return Ok();
        }

    }
}
