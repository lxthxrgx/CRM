using Microsoft.AspNetCore.Mvc;
using SRMAgreement.Class;
using SRMAgreement.Data_Base;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SRMAgreement.Controllers
{
    public class ModalWindowGroupsPage : Controller
    {
        private readonly DataBaseContext _context;

        public ModalWindowGroupsPage(DataBaseContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                var model = _context.SubleaseDop.Find(id.Value);
                if (model == null)
                {
                    return NotFound();
                }
                return View(model);
            }
            return View(new SubleaseDop());
        }

        [HttpPost]
        public IActionResult Save(SubleaseDop model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            if (model.Id == 0)
            {
                _context.SubleaseDop.Add(model);
            }
            else
            {
                _context.SubleaseDop.Update(model);
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [Route("ModalWindowGroupsPage/GetDataById/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetDataById(int id)
        {
            var numberGroup = await _context.D2
                .Where(item => item.Id == id)
                .Select(x => x.NumberGroup)
                .FirstOrDefaultAsync();

            if (numberGroup == null)
            {
                return Json(new { success = false, message = $"NumberGroup не найден для Id: {id}" });
            }

            var testviewTest = await _context.SubleaseDop
                .FirstOrDefaultAsync(x => x.NumberGroup == numberGroup);

            if (testviewTest == null)
            {
                return Json(new { success = false, message = $"Запись с NumberGroup = {numberGroup} не найдена в SubleaseDop." });
            }

            return Json(new
            {
                success = true,
                data = new
                {
                    id = testviewTest.Id,
                    date = testviewTest.Date?.ToString("yyyy-MM-dd"),
                    num = testviewTest.Num,
                    name = testviewTest.Name,
                    rnokpp = testviewTest.rnokpp,
                    status = testviewTest.status,
                    Certificate_Series = testviewTest.Certificate_Series,
                    Certificate_Number = testviewTest.Certificate_Number,
                    Issued = testviewTest.Issued,
                    Registration_Date = testviewTest.Registration_Date,
                    Index_Number = testviewTest.Index_Number,
                    Status_Record = testviewTest.Status_Record
                }
            });
        }

        [Route("ModalWindowGroupsPage/AddRentData")]
        [HttpPost]
        public async Task<IActionResult> AddRentData([FromBody] SubleaseDop model)
        {
            if (model == null)
            {
                return BadRequest(new { success = false, message = "No data provided" });
            }
            var existingSubleaseDop = await _context.SubleaseDop
                .FirstOrDefaultAsync(x => x.NumberGroup == model.NumberGroup);

            if (existingSubleaseDop == null)
            {
                var newSubleaseDop = new SubleaseDop
                {
                    NumberGroup = model.NumberGroup,
                    Num = model.Num,
                    Date = model.Date,
                    Name = model.Name,
                    rnokpp = model.rnokpp,
                    status = model.status,
                    Certificate_Series = model.Certificate_Series,
                    Certificate_Number = model.Certificate_Number,
                    Issued = model.Issued,
                    Registration_Date = model.Registration_Date,
                    Index_Number = model.Index_Number,
                    Status_Record = model.Status_Record
                };

                _context.SubleaseDop.Add(newSubleaseDop);
                await _context.SaveChangesAsync();

                var existing2D = await _context.D2
                    .FirstOrDefaultAsync(d => d.NumberGroup == model.NumberGroup);

                if (existing2D != null)
                {
                    existing2D.SubleaseDopId = newSubleaseDop.Id;
                    _context.D2.Update(existing2D);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return BadRequest(new { success = false, message = "No matching D2 record found to update." });
                }
            }
            else
            {
                existingSubleaseDop.Num = model.Num;
                existingSubleaseDop.Date = model.Date;
                existingSubleaseDop.Name = model.Name;
                existingSubleaseDop.rnokpp = model.rnokpp;
                existingSubleaseDop.status = model.status;
                existingSubleaseDop.Certificate_Series = model.Certificate_Series;
                existingSubleaseDop.Certificate_Number = model.Certificate_Number;
                existingSubleaseDop.Issued = model.Issued;
                existingSubleaseDop.Registration_Date = model.Registration_Date;
                existingSubleaseDop.Index_Number = model.Index_Number;
                existingSubleaseDop.Status_Record = model.Status_Record;
                _context.SubleaseDop.Update(existingSubleaseDop);
                await _context.SaveChangesAsync();
            }

            return Json(new { success = true });
        }

    }
}

