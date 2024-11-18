using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRMAgreement.Data_Base;
using WordLibrary;
using SRMAgreement.word;
using System.Text.RegularExpressions;
using System.IO;
using System.Text.Json;
using SRMAgreement.SuppCode;
using Newtonsoft.Json.Linq;
using static System.Collections.Specialized.BitVector32;
using System.Drawing;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SRMAgreement.Controllers
{
    [Authorize]
    [Route("api")]
    public class Api : Controller
    {
        private readonly IHubContext<WordHub> _hubContext;
        private readonly DataBaseContext _context;

        public Api(DataBaseContext context, IHubContext<WordHub> hubContext) { _context = context; _hubContext = hubContext; ; }

        [HttpPost("Info/ForModelView/ById/{num}")]
        public async Task<Sublease_> GetDataById(int num)
        {
            try
            {
                var department = _context.D4
                    .Where(p => p.NumberGroup == num)
                    .Select(d => new Sublease_
                    {
                        Sublease_Department_Number = d.NumberGroup.ToString() ?? string.Empty,
                        Sublease_Department_Name = d.NameGroup ?? string.Empty,
                        Sublease_Department_Address = d.address ?? string.Empty,
                        Sublease_Sublease_Agreement = d.DogovirSuborendu ?? string.Empty,
                        Sublease_Sublease_Agreement_Date = d.DateTime.ToString("dd/MM/yyyy") ?? string.Empty,
                        Sublease_Amount = d.Suma.ToString() ?? string.Empty,
                        Sublease_Early_termination = d.AktDate.ToString("dd/MM/yyyy") ?? string.Empty,
                    })
                    .FirstOrDefault();

                return department;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetDataById(API) Error: {ex.Message}");
                return null;
            }
        }

        [HttpPost("Info/ForModelView/ByName/{name}")]
        public async Task<Sublease_> GetDataByName(string name)
        {
            try
            {
                var department = _context.D4
                    .Where(p => p.NameGroup == name)
                    .Select(d => new Sublease_
                    {
                        Sublease_Department_Number = d.NumberGroup.ToString() ?? string.Empty,
                        Sublease_Department_Name = d.NameGroup ?? string.Empty,
                        Sublease_Department_Address = d.address ?? string.Empty,
                        Sublease_Sublease_Agreement = d.DogovirSuborendu ?? string.Empty,
                        Sublease_Sublease_Agreement_Date = d.DateTime.ToString("dd/MM/yyyy") ?? string.Empty,
                        Sublease_Amount = d.Suma.ToString() ?? string.Empty,
                        Sublease_Early_termination = d.AktDate.ToString("dd/MM/yyyy") ?? string.Empty,
                    })
                    .FirstOrDefault();

                return department;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetDataById(API) Error: {ex.Message}");
                return null;
            }
        }

        [HttpPost("Info/All/ById/{num}")]
        public async Task<Sublease_> GetAllById(int num)
        {
            try
            {
                var department = _context.D4
                    .Where(p => p.NumberGroup == num)
                    .Select(d => new Sublease_
                    {
                        Sublease_Department_Number = d.NumberGroup.ToString() ?? string.Empty,
                        Sublease_Department_Name = d.NameGroup ?? string.Empty,
                        Sublease_Department_Address = d.address ?? string.Empty,
                        Sublease_Sublease_Agreement = d.DogovirSuborendu ?? string.Empty,
                        Sublease_Sublease_Agreement_Date = d.DateTime.ToString("dd/MM/yyyy") ?? string.Empty,
                        Sublease_Amount = d.Suma.ToString() ?? string.Empty,
                        Sublease_Early_termination = d.AktDate.ToString("dd/MM/yyyy") ?? string.Empty,
                    })
                    .FirstOrDefault();

                return department;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetDataById(API) Error: {ex.Message}");
                return null;
            }
        }

        public class PathRequest
        {
            public string Path { get; set; }
            public string Name { get; set; }
        }

        [HttpPost("Path/ToWordFile")]
        public async Task<string> ReturnPathToWord([FromBody] PathRequest request)
        {
            try
            {
                var path = request.Path;
                var name = request.Name;
                Console.WriteLine(path + "------" + name);
                var userName = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                Console.WriteLine(userName);
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", path, userName);
                return path;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ReturnPathToWord(API) Error: {ex.Message}");
                return null;
            }
        }
    }

    public class Data
    {
        //Counterparty_
        public string Counterparty_Department_Name { get; set; } //Назва відділення
        public string Counterparty_Surname_Initials { get; set; } //ПІБ(повне)	
        public string Counterparty_Rnokpp { get; set; } //РНОКПП \ эдрпоу
        public string Counterparty_Counterparty_Address { get; set; } //Місцезнаходження
        public string Counterparty_USRLEIE { get; set; } //ЄДРЮОФОП
        public string Counterparty_Bank_Account_Details { get; set; } //Рахунок+Назва банку
        public string Counterparty_Direvtor { get; set; }  //Директор
        public string Counterparty_Responsible_Person { get; set; } //Відповідальна особа
        public string Counterparty_Responsible_Person_Phone { get; set; } //Телефон
        public string Counterparty_Responsible_Person_Emaile { get; set; } //E-mail
        public bool Counterparty_IsTov { get; set; } //Тов

        //Sublease_
        public string Sublease_Department_Number { get; set; } //Номер
        public string Sublease_Department_Name { get; set; } //Найменування
        public string Sublease_Department_Address { get; set; } //Адреса
        public string Sublease_Sublease_Agreement { get; set; } //Договір суборенди
        public string Sublease_Sublease_Agreement_Date { get; set; } //Дата укладання	
        public string Sublease_Amount { get; set; } //Сума	
        public string Sublease_Early_termination { get; set; } //Акт дата	
        public string Sublease_Less_than_year { get; set; } //меньше року
        public string Sublease_Status { get; set; } //Стан
        public string Sublease_Note { get; set; } //Примітка

        //Guard_
        public string Guard_Department_Number { get; set; } //Номер відділення
        public string Guard_Department_Address { get; set; } //Адреса
        public string Guard_Company { get; set; } //Охоронна копанія
        public string Guard_Agreement { get; set; } //Номер договору
        public string Guard_Term { get; set; } //Строк дії
        public string Guard__Responsible_Person { get; set; } //Відповідальна особа
        public string Guard_Responsible_Person_Phone { get; set; } //Телефон
        public string Guard_Path_To_Pdf { get; set; } //Pdf
    }
    public class Counterparty_ : Data
    {
        public string Counterparty_Department_Name { get; set; } //Назва відділення
        public string Counterparty_Surname_Initials { get; set; } //ПІБ(повне)	
        public string Counterparty_Rnokpp { get; set; } //РНОКПП \ эдрпоу
        public string Counterparty_Counterparty_Address { get; set; } //Місцезнаходження
        public string Counterparty_USRLEIE { get; set; } //ЄДРЮОФОП
        public string Counterparty_Bank_Account_Details { get; set; } //Рахунок+Назва банку
        public string Counterparty_Direvtor { get; set; }  //Директор
        public string Counterparty_Responsible_Person { get; set; } //Відповідальна особа
        public string Counterparty_Responsible_Person_Phone { get; set; } //Телефон
        public string Counterparty_Responsible_Person_Emaile { get; set; } //E-mail
        public bool Counterparty_IsTov { get; set; } //Тов
    }


    public class Sublease_ : Data
    {
        public string Sublease_Department_Number { get; set; } //Номер
        public string Sublease_Department_Name { get; set; } //Найменування
        public string Sublease_Department_Address { get; set; } //Адреса
        public string Sublease_Sublease_Agreement { get; set; } //Договір суборенди
        public string Sublease_Sublease_Agreement_Date { get; set; } //Дата укладання	
        public string Sublease_Amount { get; set; } //Сума	
        public string Sublease_Early_termination { get; set; } //Акт дата	
        public string Sublease_Less_than_year { get; set; } //меньше року
        public string Sublease_Status { get; set; } //Стан
        public string Sublease_Note { get; set; } //Примітка
    }

    public class Guard_ : Data
    {
        public string Guard_Department_Number { get; set; } //Номер відділення
        public string Guard_Department_Address { get; set; } //Адреса
        public string Guard_Company { get; set; } //Охоронна копанія
        public string Guard_Agreement { get; set; } //Номер договору
        public string Guard_Term { get; set; } //Строк дії
        public string Guard__Responsible_Person { get; set; } //Відповідальна особа
        public string Guard_Responsible_Person_Phone { get; set; } //Телефон
        public string Guard_Path_To_Pdf { get; set; } //Pdf
    }
}
