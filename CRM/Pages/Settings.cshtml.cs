using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using SRMAgreement.Data_Base;
using System.Text;
using System.Text.Json;

namespace SRMAgreement.Pages
{
    public class SettingsModel : PageModel
    {
        private readonly DataBase _databaseBackup;
        private readonly IWebHostEnvironment _env;

        public SettingsModel(IConfiguration configuration, IWebHostEnvironment env)
        {
            _databaseBackup = new DataBase(configuration, env);
            _env = env;
        }


        [BindProperty]
        public string FilePath { get; set; }
        [BindProperty]
        public string FilePathDoc1 { get; set; }
        [BindProperty]
        public string FilePathDoc2 { get; set; }
        [BindProperty]
        public string FilePathDoc3 { get; set; }
        [BindProperty]
        public string FilePathDoc4 { get; set; }

        [BindProperty]
        public string FilePathDoc1tov { get; set; }
        [BindProperty]
        public string FilePathDoc2tov { get; set; }
        [BindProperty]
        public string FilePathDoc3tov { get; set; }
        [BindProperty]
        public string FilePathDoc4tov { get; set; }
        //[BindProperty]
        //public string FilePathDoc_rent1 { get; set; }
        //[BindProperty]
        //public string FilePathDoc_rent2 { get; set; }
        //[BindProperty]
        //public string FilePathDoc_rent3 { get; set; }
        //[BindProperty]
        //public string FilePathDoc_rent4 { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPostBackup()
        {
            string backupFilePath = _databaseBackup.BackupDatabase();

            if (System.IO.File.Exists(backupFilePath))
            {
                var fileName = Path.GetFileName(backupFilePath);
                var mimeType = "application/x-sqlite3";
                return PhysicalFile(backupFilePath, mimeType, fileName);
            }
            else
            {
                return NotFound();
            }
        }


        public IActionResult OnPostSample()
        {
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            if (!System.IO.File.Exists(configFilePath))
            {
                System.IO.File.WriteAllText(configFilePath, "{}");
            }

            var json = System.IO.File.ReadAllText(configFilePath, Encoding.UTF8);
            var jObject = JObject.Parse(json);
            var customSettings = jObject["CustomSettings"] as JObject ?? new JObject();
            customSettings["path"] = FilePath;

            jObject["CustomSettings"] = customSettings;

            var updatedJson = jObject.ToString(Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(configFilePath, updatedJson, Encoding.UTF8);

            return RedirectToPage();
        }


        public IActionResult OnPostSaveAll()
        {
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            if (!System.IO.File.Exists(configFilePath))
            {
                System.IO.File.WriteAllText(configFilePath, "{}");
            }

            var json = System.IO.File.ReadAllText(configFilePath, Encoding.UTF8);
            var jObject = JObject.Parse(json);
            var customSettings = jObject["sublease"] as JObject ?? new JObject();

            customSettings["sublease-agreement"] = FilePathDoc1;
            customSettings["sublease-act"] = FilePathDoc2;
            customSettings["sublease-termination"] = FilePathDoc3;
            customSettings["sublease-return-act"] = FilePathDoc4;

            jObject["sublease"] = customSettings;

            var updatedJson = jObject.ToString(Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(configFilePath, updatedJson, Encoding.UTF8);

            return RedirectToPage();
        }

        public IActionResult OnPostSaveAllTOV()
        {
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            if (!System.IO.File.Exists(configFilePath))
            {
                System.IO.File.WriteAllText(configFilePath, "{}");
            }

            var json = System.IO.File.ReadAllText(configFilePath, Encoding.UTF8);
            var jObject = JObject.Parse(json);
            var customSettings = jObject["sublease-tov"] as JObject ?? new JObject();

            customSettings["sublease-agreement-tov"] = FilePathDoc1tov;
            customSettings["sublease-act-tov"] = FilePathDoc2tov;
            customSettings["sublease-termination-tov"] = FilePathDoc3tov;
            customSettings["sublease-return-act-tov"] = FilePathDoc4tov;

            jObject["sublease-tov"] = customSettings;

            var updatedJson = jObject.ToString(Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(configFilePath, updatedJson, Encoding.UTF8);

            return RedirectToPage();
        }
    }
}
