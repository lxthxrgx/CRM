using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRMAgreement.Class;
using SRMAgreement.Data_Base;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SRMAgreement.Controllers
{
    [Route("[controller]/[action]")]
    public class _2DController : Controller
    {
        private readonly DataBaseContext _context;

        public _2DController(DataBaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDataFromDBNumGroup(int num)
        {
            var data = await _context.D2
                .Where(x => x.NumberGroup == num)
                .Select(x => x.NameGroup)
                .ToListAsync();

            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAdditionalData(int num)
        {
            var data = await _context.D2
                .Where(x => x.NumberGroup == num)
                .Select(x => x.address)
                .ToListAsync();

            return Json(data);
        }
        [HttpGet]
        public async Task<IActionResult> GetDataForDropdown4(int num)
        {
            var data = await _context.D2
                .Where(x => x.NumberGroup == num)
                .Select(x => x.area)
                .ToListAsync();

            return Json(data);
        }
        [HttpGet]
        public async Task<IActionResult> GetPhonesByResPerson(string resPerson)
        {
            var data = await _context.D1
                .Where(x => x.ResPerson == resPerson)
                .Select(x => x.Phone)
                .ToListAsync();

            return Json(data);
        }
        [HttpGet]
        public async Task<IActionResult> GetAdditionalDataPIBS(int num)
        {
            var data = await _context.D2
                .Where(x => x.NumberGroup == num)
                .Select(x => x.PIBS)
                .ToListAsync();

            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAddressesByNumberGroup(int numberGroup)
        {
            var data = await _context.D2
                .Where(x => x.NumberGroup == numberGroup)
                .Select(x => x.address)
                .ToListAsync();

            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetPIBByAddress(string address)
        {
            var data = await _context.D2
                .Where(x => x.address == address)
                .Select(x => x.PIBS)
                .ToListAsync();

            return Json(data);
        }

        [HttpPost("PostDataFromD2")]
        public async Task<IActionResult> PostDataFromD2([FromForm] List<_2D> D2)
        {
            if (D2 == null || D2.Count == 0)
            {
                return BadRequest("D2 cannot be null or empty.");
            }

            // Выводим для отладки
            Console.WriteLine($"Received {D2.Count} items.");

            foreach (var item in D2)
            {
                if (item.NumberGroup == 0 ||
                    string.IsNullOrEmpty(item.NameGroup) ||
                    string.IsNullOrEmpty(item.PIBS) ||
                    string.IsNullOrEmpty(item.address) ||
                    item.area == 0 ||
                    string.IsNullOrEmpty(item.rent) ||
                    item.isAlert == null ||
                    item.DateCloseDepartment == null)
                {
                    return BadRequest("Не всі дані заповнені!!!");
                }

                try
                {
                    if (item.Id == 0)
                    {
                        _context.D2.Add(item);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        var entity = await _context.D2.FindAsync(item.Id);
                        if (entity != null)
                        {
                            entity.NumberGroup = item.NumberGroup;
                            entity.NameGroup = item.NameGroup;
                            entity.PIBS = item.PIBS;
                            entity.address = item.address;
                            entity.area = item.area;
                            entity.isAlert = item.isAlert;
                            entity.DateCloseDepartment = item.DateCloseDepartment;

                            await _context.SaveChangesAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Ошибка при обработке данных: " + ex.Message);
                }
            }

            return Ok(new { status = true, message = "Дані збережено успішно" });
        }
    }
}
