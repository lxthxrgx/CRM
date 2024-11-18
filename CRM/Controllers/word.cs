using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRMAgreement.Data_Base;
using WordLibrary;
using SRMAgreement.word;
using System.Text.RegularExpressions;
using System.Text.Json;
using SRMAgreement.SuppCode;
using Newtonsoft.Json.Linq;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using SRMAgreement.Class;

namespace SRMAgreement.Controllers
{

    [Authorize]
    [Route("word")]
    public class WordController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataBaseContext _context;

        public WordController(DataBaseContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public static string GetShortenedName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException("Full name cannot be null or empty.", nameof(fullName));
            }

            var parts = fullName.Split(' ');

            if (parts.Length < 3)
            {
                throw new ArgumentException("Full name must include at least three parts: last name, first name, and middle name.", nameof(fullName));
            }

            var lastName = parts[0];
            var firstNameInitial = parts[1][0];
            var middleNameInitial = parts[2][0];

            return $"{lastName} {firstNameInitial}.{middleNameInitial}.";
        }

        public string RenameGog(string Name)
        {
            string output = Regex.Replace(Name, "/", "_");
            return output;
        }

        private static string LoadSettingValue(string key)
        {
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            var json = System.IO.File.ReadAllText(configFilePath);
            var jObject = JObject.Parse(json);
            string section = "CustomSettings";
            var myCustomSettings = jObject[section];
            if (myCustomSettings == null)
            {
                return null;
            }

            var valueToken = myCustomSettings[key];
            return valueToken?.ToString();
        }

        public static string CreateDirectoryForDoc(string NameGroup2D)
        {
            try
            {
                var pathFromSettings = LoadSettingValue("path");
                if (pathFromSettings == null)
                {
                    Console.WriteLine("Path not found in settings.json.");
                    return null;
                }
                var sanitizedDirectoryName = SanitizeDirectoryName(NameGroup2D);

                var full_path = Path.Combine(pathFromSettings, sanitizedDirectoryName);

                if (Directory.Exists(full_path))
                {
                    Console.WriteLine("That path exists already.");
                    return full_path;
                }

                DirectoryInfo di = Directory.CreateDirectory(full_path);
                Console.WriteLine($"Directory created successfully at {di.FullName}");
                return di.FullName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The process failed: {ex.ToString()}");
                return null;
            }
        }
        public static string CreateDirectoryForDocGuard(string NameGroup2D)
        {
            try
            {
                var pathFromSettings = LoadSettingValue("path");
                if (pathFromSettings == null)
                {
                    Console.WriteLine("Path not found in settings.json.");
                    return null;
                }
                var sanitizedDirectoryName = SanitizeDirectoryName(NameGroup2D);

                var mainDirectoryPath = Path.Combine(pathFromSettings, sanitizedDirectoryName);

                if (!Directory.Exists(mainDirectoryPath))
                {
                    DirectoryInfo mainDirectory = Directory.CreateDirectory(mainDirectoryPath);
                    Console.WriteLine($"Main directory created successfully at {mainDirectory.FullName}");
                }
                else
                {
                    Console.WriteLine("Main directory exists already.");
                }

                var subDirectoryPath = Path.Combine(mainDirectoryPath, "Охорона");
                Console.WriteLine("subDirectoryPath--------------------------" + subDirectoryPath);
                if (!Directory.Exists(subDirectoryPath))
                {
                    DirectoryInfo subDirectory = Directory.CreateDirectory(subDirectoryPath);
                    Console.WriteLine($"Subdirectory created successfully at {subDirectory.FullName}");
                }
                else
                {
                    Console.WriteLine("Subdirectory exists already.");
                }

                return subDirectoryPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The process failed: {ex.ToString()}");
                return null;
            }
        }
        public static string CreateDirectoryForSubleasePDF(string NameGroup2D)
        {
            try
            {
                var pathFromSettings = LoadSettingValue("path");
                if (pathFromSettings == null)
                {
                    Console.WriteLine("Path not found in settings.json.");
                    return null;
                }
                var sanitizedDirectoryName = SanitizeDirectoryName(NameGroup2D);

                var mainDirectoryPath = Path.Combine(pathFromSettings, sanitizedDirectoryName);

                if (!Directory.Exists(mainDirectoryPath))
                {
                    DirectoryInfo mainDirectory = Directory.CreateDirectory(mainDirectoryPath);
                    Console.WriteLine($"Main directory created successfully at {mainDirectory.FullName}");
                }
                else
                {
                    Console.WriteLine("Main directory exists already.");
                }

                var subDirectoryPath = Path.Combine(mainDirectoryPath, "PDF Суборенда");
                Console.WriteLine("subDirectoryPath--------------------------" + subDirectoryPath);
                if (!Directory.Exists(subDirectoryPath))
                {
                    DirectoryInfo subDirectory = Directory.CreateDirectory(subDirectoryPath);
                    Console.WriteLine($"Subdirectory created successfully at {subDirectory.FullName}");
                }
                else
                {
                    Console.WriteLine("Subdirectory exists already.");
                }

                return subDirectoryPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The process failed: {ex.ToString()}");
                return null;
            }
        }

        private static string SanitizeDirectoryName(string name)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var sanitized = new string(name
                .Where(ch => !invalidChars.Contains(ch))
                .ToArray());
            return sanitized;
        }

        private static string LoadSettingSublease(string key, string treekey)
        {
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            var json = System.IO.File.ReadAllText(configFilePath);
            var jObject = JObject.Parse(json);
            string section = treekey;
            var myCustomSettings = jObject[section];
            if (myCustomSettings == null)
            {
                return null;
            }

            var valueToken = myCustomSettings[key];
            return valueToken?.ToString();
        }

        public string Doublezero(double num)
        {
            return $"{num:N2}".Replace('.', ',');
        }

        public class PathRequest //для Апи
        {
            public string Path { get; set; }
            public string Name { get; set; }
        }

        [HttpPost("OnPostGenerateWordAsync")]
        public async Task<IActionResult> OnPostGenerateWordAsync([FromForm] string NameGroup, [FromForm] int NumberGroup,
            [FromForm] DateTime EndAktDate, [FromForm] string Suma, [FromForm] DateTime DateTime,
            [FromForm] string DogovirSuborendu, [FromForm] DateTime aktDate)
        {
            bool? IsTovNullable = await _context.D1.Where(p => p.NameGroup == NameGroup).Select(x => x.Tov).FirstOrDefaultAsync();
            bool IsTov = IsTovNullable.HasValue;

            var relatedDataFullname = await _context.D1
            .Where(p => p.NameGroup == NameGroup)
            .Select(x => x.Fullname)
            .FirstOrDefaultAsync() ?? "default_fullname";

            var relatedDataList = await _context.D1
            .Where(p => p.NameGroup == NameGroup)
            .Select(x => x.rnokpp)
            .FirstOrDefaultAsync();

            var relatedDataedryofop_Data = await _context.D1
            .Where(p => p.NameGroup == NameGroup)
            .Select(x => x.edryofop_Data)
            .FirstOrDefaultAsync() ?? "default_edryofop_Data";

            var relatedDataBanckAccount = await _context.D1
            .Where(p => p.NameGroup == NameGroup)
            .Select(x => x.BanckAccount)
            .FirstOrDefaultAsync() ?? "default_BanckAccount";

            var relatedDataaddress = await _context.D1
            .Where(p => p.NameGroup == NameGroup)
            .Select(x => x.address)
            .FirstOrDefaultAsync() ?? "default_address_p";

            var relatedDataarea = await _context.D2
            .Where(p => p.NumberGroup == NumberGroup)
            .Select(x => x.area)
            .FirstOrDefaultAsync();

            var relatedDataNameGroup = await _context.D2
            .Where(p => p.NumberGroup == NumberGroup)
            .Select(x => x.NameGroup)
            .FirstOrDefaultAsync() ?? "relatedDataNameGroup";

            var relatedDataPIBS = await _context.D2
            .Where(p => p.NameGroup == NameGroup)
            .Select(x => x.PIBS)
            .FirstOrDefaultAsync() ?? "relatedDataPIBS";

            var address = await _context.D4
            .Where(p => p.NumberGroup == NumberGroup)
            .Select(x => x.address)
            .FirstOrDefaultAsync();

            var Aktdate = await _context.D4
            .Where(p => p.NumberGroup == NumberGroup)
            .Select(x => x.AktDate)
            .FirstOrDefaultAsync();

            var subleaseDopNum = await _context.D2
            .Where(p => p.NumberGroup == NumberGroup)
            .Select(p => p.SubleaseDop.Num)
            .FirstOrDefaultAsync();

            var subleaseDopDate = await _context.D2
                .Where(p => p.NumberGroup == NumberGroup)
                .Select(p => p.SubleaseDop.Date)
            .FirstOrDefaultAsync();

            var subleaseDopName = await _context.D2
            .Where(p => p.NumberGroup == NumberGroup)
            .Select(p => p.SubleaseDop.Name)
            .FirstOrDefaultAsync();

            var subleaseDopRnokpp = await _context.D2
            .Where(p => p.NumberGroup == NumberGroup)
            .Select(p => p.SubleaseDop.rnokpp)
            .FirstOrDefaultAsync();

            var subleaseDopstatus = await _context.D2
            .Where(p => p.NumberGroup == NumberGroup)
             .Select(p => p.SubleaseDop.status)
            .FirstOrDefaultAsync();


            Console.WriteLine("------------------- \n IS IT TOV: " + IsTov + "---------------------- \n");

            var Status_for_word_generation = await _context.D2
                .Include(x => x.SubleaseDop)
                .Where(x => x.NumberGroup == NumberGroup)
                .Select(x => x.SubleaseDop.Status_Record)
                .FirstOrDefaultAsync();
            Console.WriteLine("Status_for_word_generation---------------> " + Status_for_word_generation);

            if (string.IsNullOrEmpty(Status_for_word_generation))
            {
                return BadRequest(new { error = "В групах потрібно вибрати 'Суборенда' або 'Оренда'" });
            }

            if (Status_for_word_generation == "Sublease")
            {
                //SUBLEASE START Dog+Akt
                if (IsTov)
                {
                    try
                    {
                        var dirPath = CreateDirectoryForDoc($"{NumberGroup}-{NameGroup}");
                        var path_dog = await wordDoc.CopyOriginalFile(LoadSettingSublease("sublease-agreement-tov", "sublease-tov"), $"{RenameGog(DogovirSuborendu)}-{relatedDataNameGroup}-договір-ТОВ", dirPath);
                        var Director = _context.D1
                            .Where(p => p.NameGroup == NameGroup)
                            .Select(x => x.Director)
                            .FirstOrDefault() ?? "default_Director";

                        var rnokppValue = await _context.SubleaseDop
                          .Where(x => x.NumberGroup == NumberGroup)
                          .Select(x => x.rnokpp)
                          .FirstOrDefaultAsync();

                        var edrpoyValue = await _context.SubleaseDop
                            .Where(x => x.NumberGroup == NumberGroup)
                            .Select(x => x.status)
                            .FirstOrDefaultAsync();

                        var rnokpp = !string.IsNullOrEmpty(rnokppValue) ? "РНОКПП " + rnokppValue : null;
                        var edrpoy = !string.IsNullOrEmpty(edrpoyValue) ? "ЄДРПОУ " + edrpoyValue : null;

                        string? subleaseDopRnokppLastData = "_____";

                        if (string.IsNullOrEmpty(rnokpp))
                        {
                            subleaseDopRnokppLastData = edrpoy;
                        }
                        else if (string.IsNullOrEmpty(edrpoy))
                        {
                            subleaseDopRnokppLastData = rnokpp;
                        }
                        else
                        {
                            subleaseDopRnokppLastData = "ERROR";
                        }

                        var PIBSDirector = GetShortenedName(Director);
                        try
                        {
                            var b = WordHelper.AddTextToContentControl(path_dog,
                            DogovirSuborendu,
                            DateTime.ToString("dd/MM/yyyy"),
                            relatedDataFullname,
                            relatedDataedryofop_Data,
                            relatedDataList.ToString(),
                            relatedDataarea.ToString(),
                            address,
                            EndAktDate.ToString("dd/MM/yyyy"),
                            relatedDataaddress,
                            null,
                            relatedDataBanckAccount,
                            relatedDataPIBS,
                            Num_To_Text.NumberToText(Convert.ToDouble(relatedDataarea)),
                            Num_To_Text.SumToText(Convert.ToDouble(Suma)),
                            Doublezero(Convert.ToDouble(Suma)),
                            null,
                            Director,
                            PIBSDirector,
                            subleaseDopNum ?? "____",
                            subleaseDopDate?.ToString("dd/MM/yyyy") ?? "____",
                            subleaseDopName ?? "____",
                            subleaseDopRnokppLastData ?? "____",
                            subleaseDopstatus ?? "____");
                        }
                        catch (Exception ex) { Console.WriteLine($"Less_than_year error: {ex.Message}"); }



                        //"C:\\work\\wwwroot\\word\\2_Акт_суборенда.docx"
                        var path_akt = await wordDoc.CopyOriginalFile(LoadSettingSublease("sublease-act-tov", "sublease-tov"), $"{RenameGog(DogovirSuborendu)}-{relatedDataNameGroup}-акт-ТОВ", dirPath);
                        var a = WordHelper.AddTextToContentControl(path_akt,
                           DogovirSuborendu,
                           DateTime.ToString("dd/MM/yyyy"),
                           relatedDataFullname,
                           relatedDataedryofop_Data,
                           relatedDataList.ToString(),
                           relatedDataarea.ToString(),
                           address,
                           EndAktDate.ToString("dd/MM/yyyy"),
                           relatedDataaddress,
                           null,
                           relatedDataBanckAccount,
                           relatedDataPIBS,
                           Num_To_Text.NumberToText(Convert.ToDouble(relatedDataarea)),
                           Num_To_Text.SumToText(Convert.ToDouble(Suma)),
                           Doublezero(Convert.ToDouble(Suma)),
                           Num_To_Text.NumberMonthToText(aktDate.ToString("dd/MM/yyyy")),
                           Director,
                           PIBSDirector,
                           null,
                           null,
                           null,
                           null,
                           null);
                        Console.WriteLine("Document generation successful.");
                        return Ok(new { success = "Документи (договір + акт) згенеровані для ТОВ" });

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"OnPostGenerateWordAsync Error generating document: {ex.Message}");
                    }
                }
                else
                {
                    try
                    {
                        var dirPath = CreateDirectoryForDoc($"{NumberGroup}-{NameGroup}");
                        //"C:\\work\\wwwroot\\word\\1_Дог_ суборенда_ФОП_orig.docx"

                        Console.WriteLine(LoadSettingSublease("sublease-agreement", "sublease"));

                        var path_dog = await wordDoc.CopyOriginalFile(LoadSettingSublease("sublease-agreement", "sublease"), $"{RenameGog(DogovirSuborendu)}-{relatedDataNameGroup}-договір", dirPath);

                        var rnokppValue = await _context.SubleaseDop
                         .Where(x => x.NumberGroup == NumberGroup)
                         .Select(x => x.rnokpp)
                         .FirstOrDefaultAsync();

                        var edrpoyValue = await _context.SubleaseDop
                            .Where(x => x.NumberGroup == NumberGroup)
                            .Select(x => x.status)
                            .FirstOrDefaultAsync();

                        var rnokpp = !string.IsNullOrEmpty(rnokppValue) ? "РНОКПП " + rnokppValue : null;
                        var edrpoy = !string.IsNullOrEmpty(edrpoyValue) ? "ЄДРПОУ " + edrpoyValue : null;

                        var RnokppFromDb = await _context.D1
                            .Where(p => p.NameGroup == NameGroup)
                            .Select(x => x.rnokpp)
                            .FirstOrDefaultAsync();

                        string? subleaseDopRnokppLastData = "_____";

                        if (string.IsNullOrEmpty(rnokpp))
                        {
                            subleaseDopRnokppLastData = edrpoy;
                        }
                        else if (string.IsNullOrEmpty(edrpoy))
                        {
                            subleaseDopRnokppLastData = rnokpp;
                        }
                        else
                        {
                            subleaseDopRnokppLastData = "ERROR";
                        }

                        try
                        {

                            var b = WordHelper.AddTextToContentControl(path_dog,
                            DogovirSuborendu,
                            DateTime.ToString("dd/MM/yyyy"),
                            relatedDataFullname,
                            relatedDataedryofop_Data,
                            RnokppFromDb,
                            relatedDataarea.ToString(),
                            address,
                            EndAktDate.ToString("dd/MM/yyyy"),
                            relatedDataaddress,
                            null,
                            relatedDataBanckAccount,
                            relatedDataPIBS,
                            Num_To_Text.NumberToText(Convert.ToDouble(relatedDataarea)),
                            Num_To_Text.SumToText(Convert.ToDouble(Suma)).ToString(),
                            Doublezero(Convert.ToDouble(Suma)),
                            null,
                            null,
                            null,
                            subleaseDopNum ?? "____",
                            subleaseDopDate?.ToString("dd/MM/yyyy") ?? "____",
                            subleaseDopName ?? "____",
                            subleaseDopRnokppLastData ?? "____",
                            subleaseDopstatus ?? "____");
                        }
                        catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }

                        //"C:\\work\\wwwroot\\word\\2_Акт_суборенда.docx"
                        var path_akt = await wordDoc.CopyOriginalFile(LoadSettingSublease("sublease-act", "sublease"), $"{RenameGog(DogovirSuborendu)}-{relatedDataNameGroup}-акт", dirPath);
                        var a = WordHelper.AddTextToContentControl(path_akt,
                            DogovirSuborendu,
                            DateTime.ToString("dd/MM/yyyy"),
                            relatedDataFullname,
                            relatedDataedryofop_Data,
                            RnokppFromDb,
                            relatedDataarea.ToString(),
                            address,
                            EndAktDate.ToString("dd/MM/yyyy"),
                            relatedDataaddress,
                            null,
                            relatedDataBanckAccount,
                            relatedDataPIBS,
                            Num_To_Text.NumberToText(Convert.ToDouble(relatedDataarea)),
                            Num_To_Text.SumToText(Convert.ToDouble(Suma)),
                            Doublezero(Convert.ToDouble(Suma)),
                            Num_To_Text.NumberMonthToText(aktDate.ToString("dd/MM/yyyy")),
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null
                        );
                        Console.WriteLine("Document generation successful.");
                        return Ok(new { success = "Документи (договір + акт) згенеровані для ФОП" });

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"OnPostGenerateWordAsync1 Error generating document: {ex.Message}");
                    }
                }
                //SUBLEASE END Dog+Akt
            }
            if (Status_for_word_generation == "Rent-1")
            {
                //RENT START Dog+Akt

                var Director = _context.D1
                    .Where(p => p.NameGroup == NameGroup)
                    .Select(x => x.Director)
                    .FirstOrDefault() ?? "default_Director";

                var PIBSDirector = GetShortenedName(Director);

                if (IsTov)
                {
                    var dirPath = CreateDirectoryForDoc($"{NumberGroup}-{NameGroup}");
                    var path_rent_dog_tov = await wordDoc.CopyOriginalFile(LoadSettingSublease("rent-dog-tov", "rent-tov"), $"{RenameGog(DogovirSuborendu)}-{relatedDataNameGroup}-договір-оренди", dirPath);

                    var ForNaPid_Data = await _context.SubleaseDop.
                        Where(x => x.NumberGroup == NumberGroup).FirstOrDefaultAsync();

                    string NaPid = $"свідоцтва про право власності серії {ForNaPid_Data.Certificate_Series} номер {ForNaPid_Data.Certificate_Number}, виданого {ForNaPid_Data.Issued}.";

                    var a = Rent_Dog_Tov.AddTextToContentControl(
                        path_rent_dog_tov, // -> docPath
                        DogovirSuborendu, // -> DogovirRent
                        DateTime.ToString("dd/MM/yyyy"), // -> DateTime
                        relatedDataList.ToString(), // -> rnokpp
                        address, // -> address
                        Director, // -> Director
                        relatedDataarea.ToString(), // -> area
                        Num_To_Text.NumberToText(Convert.ToDouble(relatedDataarea)), // -> area_text
                        relatedDataaddress, // -> address_p
                        EndAktDate.ToString("dd/MM/yyyy"), // -> StrokDii
                        Doublezero(Convert.ToDouble(Suma)), // -> suma
                        Num_To_Text.SumToText(Convert.ToDouble(Suma)), // -> sum_text
                        relatedDataFullname, // -> PIB
                        relatedDataBanckAccount, // -> BanckAccount
                        PIBSDirector,
                        NaPid); // -> PIBSDirector

                    var path_rent_akt_tov = await wordDoc.CopyOriginalFile(LoadSettingSublease("rent-act-tov", "rent-tov"), $"{RenameGog(DogovirSuborendu)}-{relatedDataNameGroup}-акт-оренди", dirPath);

                    var b = Rent_Akt_Tov.AddTextToContentControl(
                        path_rent_akt_tov, // -> docPath
                        Num_To_Text.NumberMonthToText(aktDate.ToString("dd/MM/yyyy")), // -> DogovirRent
                        relatedDataFullname,
                        relatedDataList.ToString(), // -> rnokpp
                        relatedDataaddress, // -> address
                        address, // -> address_p
                        Director, // -> Director
                        DogovirSuborendu,
                        DateTime.ToString("dd/MM/yyyy"), // -> DateTime
                        relatedDataarea.ToString(), // -> area
                        Num_To_Text.NumberToText(Convert.ToDouble(relatedDataarea)), // -> area_text
                        relatedDataBanckAccount, // -> BanckAccount
                        PIBSDirector); // -> PIBSDirector
                    return Ok(new { success = "Документ для оренди (за свідотцтвом) успішно згенеровано!" });
                }
            }
            if (Status_for_word_generation == "Rent-2")
            {
                var Director = _context.D1
                    .Where(p => p.NameGroup == NameGroup)
                    .Select(x => x.Director)
                    .FirstOrDefault() ?? "Jhon Doe Di";

                var PIBSDirector = GetShortenedName(Director);

                if (IsTov)
                {
                    var dirPath = CreateDirectoryForDoc($"{NumberGroup}-{NameGroup}");
                    var path_rent_dog_tov = await wordDoc.CopyOriginalFile(LoadSettingSublease("rent-dog-tov", "rent-tov"), $"{RenameGog(DogovirSuborendu)}-{relatedDataNameGroup}-договір-оренди", dirPath);

                    var ForNaPid_Data = await _context.SubleaseDop.
                        Where(x => x.NumberGroup == NumberGroup).FirstOrDefaultAsync();

                    string NaPid = $"Витягу з ЄДР речових прав на нерухоме майно про реєстрацію прав та їх обтяжень від {ForNaPid_Data.Registration_Date.Value.ToString("dd/MM/yyyy")} індексний номер {ForNaPid_Data.Index_Number}";

                    var a = Rent_Dog_Tov.AddTextToContentControl(
                        path_rent_dog_tov, // -> docPath
                        DogovirSuborendu, // -> DogovirRent
                        DateTime.ToString("dd/MM/yyyy"), // -> DateTime
                        relatedDataList.ToString(), // -> rnokpp
                        address, // -> address
                        Director, // -> Director
                        relatedDataarea.ToString(), // -> area
                        Num_To_Text.NumberToText(Convert.ToDouble(relatedDataarea)), // -> area_text
                        relatedDataaddress, // -> address_p
                        EndAktDate.ToString("dd/MM/yyyy"), // -> StrokDii
                        Doublezero(Convert.ToDouble(Suma)), // -> suma
                        Num_To_Text.SumToText(Convert.ToDouble(Suma)), // -> sum_text
                        relatedDataFullname, // -> PIB
                        relatedDataBanckAccount, // -> BanckAccount
                        PIBSDirector,
                        NaPid); // -> PIBSDirector

                    var path_rent_akt_tov = await wordDoc.CopyOriginalFile(LoadSettingSublease("rent-act-tov", "rent-tov"), $"{RenameGog(DogovirSuborendu)}-{relatedDataNameGroup}-акт-оренди", dirPath);

                    var b = Rent_Akt_Tov.AddTextToContentControl(
                        path_rent_akt_tov, // -> docPath
                        Num_To_Text.NumberMonthToText(aktDate.ToString("dd/MM/yyyy")), // -> DogovirRent
                        relatedDataFullname,
                        relatedDataList.ToString(), // -> rnokpp
                        relatedDataaddress, // -> address
                        address, // -> address
                        Director, // -> Director
                        DogovirSuborendu,
                        DateTime.ToString("dd/MM/yyyy"), // -> DateTime
                        relatedDataarea.ToString(), // -> area
                        Num_To_Text.NumberToText(Convert.ToDouble(relatedDataarea)), // -> area_text
                        relatedDataBanckAccount, // -> BanckAccount
                        PIBSDirector); // -> PIBSDirector

                    return Ok(new { success = "Документ для оренди (за витягом) успішно згенеровано!" });
                }
                else
                {
                    return BadRequest(new { error = "Rent - 2 not avalible for FOP" });
                }

            }
            return BadRequest(new { error = "Помилка при стоврені документа" });
            //RENT END Dog+Akt
        }

        [HttpPost("OnPostGenerateWordAsyncPrup")]
        public async Task<IActionResult> OnPostGenerateWordAsyncPrup([FromForm] string NameGroup, [FromForm] int NumberGroup,
            [FromForm] DateTime EndAktDate, [FromForm] string Suma, [FromForm] DateTime DateTime,
            [FromForm] string DogovirSuborendu)
        {
            var relatedDataFullname = await _context.D1
                .Where(p => p.NameGroup == NameGroup)
                .Select(x => x.Fullname)
                .FirstOrDefaultAsync();

            var RnokppFromDb = await _context.D1
                .Where(p => p.NameGroup == NameGroup)
                .Select(x => x.rnokpp)
                .FirstOrDefaultAsync();

            var relatedDataedryofop_Data = await _context.D1
                .Where(p => p.NameGroup == NameGroup)
                .Select(x => x.edryofop_Data)
                .FirstOrDefaultAsync();

            var relatedDataBanckAccount = await _context.D1
                .Where(p => p.NameGroup == NameGroup)
                .Select(x => x.BanckAccount)
                .FirstOrDefaultAsync();

            var relatedDataaddress = await _context.D1
                .Where(p => p.NameGroup == NameGroup)
                .Select(x => x.address)
                .FirstOrDefaultAsync();

            var relatedDataarea = await _context.D2
                .Where(p => p.NumberGroup == NumberGroup)
                .Select(x => x.area)
                .FirstOrDefaultAsync();

            var relatedDataNameGroup = await _context.D2
                .Where(p => p.NumberGroup == NumberGroup)
                .Select(x => x.NameGroup)
                .FirstOrDefaultAsync();

            var relatedDataPIBS = await _context.D2
                .Where(p => p.NumberGroup == NumberGroup)
                .Select(x => x.PIBS)
                .FirstOrDefaultAsync();

            var address = await _context.D4
              .Where(p => p.NumberGroup == NumberGroup)
              .Select(x => x.address)
              .FirstOrDefaultAsync();

            var AktDate = _context.D4
                .Where(p => p.NumberGroup == NumberGroup)
                .Select(x => x.AktDate)
                .FirstOrDefault().ToString("dd/MM/yyyy");

            var IsTovNullable = await _context.D1
                    .Where(p => p.NameGroup == NameGroup)
                    .Select(x => x.Tov)
                    .FirstOrDefaultAsync();

            bool IsTov = IsTovNullable.HasValue && IsTovNullable.Value == true;

            var Status_for_word_generation = await _context.D2
               .Include(x => x.SubleaseDop)
               .Where(x => x.NumberGroup == NumberGroup)
               .Select(x => x.SubleaseDop.Status_Record)
               .FirstOrDefaultAsync();

            Console.WriteLine("IS IT TOV FOR OnPostGenerateWordAsyncPrup: " + IsTov);
            Console.WriteLine("Status_for_word_generation---------------> " + Status_for_word_generation);

            if (string.IsNullOrEmpty(Status_for_word_generation))
            {
                return BadRequest(new { error = "В групах потрібно вибрати 'Суборенда' або 'Оренда'" });
            }

            if (Status_for_word_generation == "Sublease")
            {
                if (IsTov)
                {
                    try
                    {
                        var dirPath = CreateDirectoryForDoc($"{NumberGroup}-{NameGroup}");
                        //"C:\\work\\wwwroot\\word\\3_ДУ_дог_субор_припинення_ФОП.docx"
                        var path_dog = await wordDoc.CopyOriginalFile(LoadSettingSublease("sublease-termination-tov", "sublease-tov"), $"{RenameGog(DogovirSuborendu)}-{relatedDataNameGroup}-припинення-ТОВ", dirPath);
                        var Director = _context.D1
                           .Where(p => p.NameGroup == NameGroup)
                           .Select(x => x.Director)
                           .FirstOrDefault() ?? "default_Director";

                        var PIBSDirector = GetShortenedName(Director);

                        var a = Sublease_Prup_Tov.AddTextToContentControl(path_dog, // -> docPath
                       DogovirSuborendu, // -> DogovirRent
                       DateTime.ToString("dd/MM/yyyy"), // -> DateTime
                       EndAktDate.AddDays(-1).ToString("dd/MM/yyyy"), // -> StrokDii
                       EndAktDate.ToString("dd/MM/yyyy"), // -> OriginalStrokDii
                       relatedDataFullname, // -> PIB
                       RnokppFromDb, // -> rnokpp
                       relatedDataaddress, // -> address 
                       Director, // ->Director
                       relatedDataarea.ToString(), // -> area
                       Num_To_Text.NumberToText(Convert.ToDouble(relatedDataarea)), // -> area_text
                       address, // -> address_p 
                       relatedDataBanckAccount, // -> BanckAccount
                       PIBSDirector); // -> PIBSDirector

                        Console.WriteLine("Document generation successful.");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error generating document: {ex.Message}");
                    }

                    return Ok(new { success = "Документ (припинення суборенди) згенеровано для ТОВ" });

                }
                if(!IsTov)
                {
                    try
                    {
                        var dirPath = CreateDirectoryForDoc($"{NumberGroup}-{NameGroup}");
                        var path_sublease_prup_fop = await wordDoc.CopyOriginalFile(LoadSettingSublease("sublease-termination", "sublease"), $"{RenameGog(DogovirSuborendu)}-{relatedDataNameGroup}-угода-про-припинення", dirPath);

                        var a = Sublease_Prup_Fop.AddTextToContentControl(path_sublease_prup_fop, // -> docPath
                            DogovirSuborendu, // -> DogovirRent
                            DateTime.ToString("dd/MM/yyyy"), // -> DateTime
                            EndAktDate.AddDays(-1).ToString("dd/MM/yyyy"), // -> StrokDii
                            EndAktDate.ToString("dd/MM/yyyy"), // -> OriginalStrokDii
                            relatedDataFullname, // -> PIB
                            RnokppFromDb, // -> rnokpp
                            relatedDataaddress, // -> address_p
                            relatedDataedryofop_Data, // -> edruofop_Data
                            relatedDataarea.ToString(), // -> area
                            Num_To_Text.NumberToText(Convert.ToDouble(relatedDataarea)), // -> area_text
                            address, // -> address
                            relatedDataBanckAccount, // -> BanckAccount
                            relatedDataPIBS); // -> PIBS
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error generating document: {ex.Message}");
                    }
                   
                }

                return Ok(new { success = "Документ (припинення суборенди) згенеровано для ФОП" });

            }

            if (Status_for_word_generation == "Rent-1" || Status_for_word_generation == "Rent-2")
            {
                if(IsTov)
                {
                    var dirPath = CreateDirectoryForDoc($"{NumberGroup}-{NameGroup}");
                    var path_rent_prup_tov = await wordDoc.CopyOriginalFile(LoadSettingSublease("rent-prup-tov", "rent-tov"), $"{RenameGog(DogovirSuborendu)}-{relatedDataNameGroup}-угода-про-припинення", dirPath);

                    var Director = _context.D1
                          .Where(p => p.NameGroup == NameGroup)
                          .Select(x => x.Director)
                          .FirstOrDefault() ?? "default_Director";
                    var PIBSDirector = GetShortenedName(Director);

                    var a = Rent_Prup_Tov.AddTextToContentControl(
                        path_rent_prup_tov, // -> docPath
                        DogovirSuborendu, // -> DogovirRent
                        DateTime.ToString("dd/MM/yyyy"), // -> DateTime
                        EndAktDate.AddDays(-1).ToString("dd/MM/yyyy"), // -> StrokDii
                        EndAktDate.ToString("dd/MM/yyyy"), // -> OriginalStrokDii
                        relatedDataFullname, // -> PIB
                        RnokppFromDb, // -> rnokpp
                        relatedDataaddress, // -> address
                        relatedDataedryofop_Data, // ->edryofop_Data
                        Director, // -> Director
                        relatedDataarea.ToString(), // -> area
                        Num_To_Text.NumberToText(Convert.ToDouble(relatedDataarea)), // -> area_text
                        address, // -> address_p 
                        relatedDataBanckAccount, // -> BanckAccount
                        PIBSDirector); // -> PIBSDirector

                    return Ok(new { success = "Документ (припинення оренди за свідотцтвом) згенеровано!" });

                }
                if(!IsTov)
                {
                    var dirPath = CreateDirectoryForDoc($"{NumberGroup}-{NameGroup}");
                    var path_rent_prup_fop = await wordDoc.CopyOriginalFile(LoadSettingSublease("rent-prup-fop", "rent-fop"), $"{RenameGog(DogovirSuborendu)}-{relatedDataNameGroup}-угода-про-припинення", dirPath);
                    var Director = _context.D1
                          .Where(p => p.NameGroup == NameGroup)
                          .Select(x => x.Director)
                          .FirstOrDefault() ?? "default_Director";

                    var a = Rent_Prup_Fop.AddTextToContentControl(path_rent_prup_fop, // -> docPath
                        DogovirSuborendu, // -> DogovirRent
                        DateTime.ToString("dd/MM/yyyy"), // -> DateTime
                        EndAktDate.AddDays(-1).ToString("dd/MM/yyyy"), // -> StrokDii
                        EndAktDate.ToString("dd/MM/yyyy"), // -> OriginalStrokDii
                        relatedDataFullname, // -> PIB
                        RnokppFromDb, // -> rnokpp
                        relatedDataaddress, // -> address
                        relatedDataedryofop_Data, // -> edruofop_Data
                        relatedDataarea.ToString(), // -> area
                        Num_To_Text.NumberToText(Convert.ToDouble(relatedDataarea)), // -> area_text
                        address, // -> address_p
                        relatedDataBanckAccount, // -> BanckAccount
                        relatedDataPIBS); // -> PIBS
                    return Ok(new { success = "Документ створено!" });
                }
                
            }
            return BadRequest(new { error = "Помилка при стоврені документа" });
        }

        [HttpPost("OnPostGenerateWordAsyncPov")]
        public async Task<IActionResult> OnPostGenerateWordAsyncPov([FromForm] string NameGroup, [FromForm] int NumberGroup,
            [FromForm] DateTime EndAktDate, [FromForm] string Suma, [FromForm] DateTime DateTime,
            [FromForm] string DogovirSuborendu)
        {
            var IsTovNullable = await _context.D1
                   .Where(p => p.NameGroup == NameGroup)
                   .Select(x => x.Tov)
                   .FirstOrDefaultAsync();

            bool IsTov = IsTovNullable.HasValue && IsTovNullable.Value == true;

            var relatedDataFullname = await _context.D1
                .Where(p => p.NameGroup == NameGroup)
                .Select(x => x.Fullname)
                .FirstOrDefaultAsync() ?? "default_fullname";

            var relatedDataList = await _context.D1
                .Where(p => p.NameGroup == NameGroup)
                .Select(x => x.rnokpp)
                .FirstOrDefaultAsync();

            var relatedDataedryofop_Data = await _context.D1
                .Where(p => p.NameGroup == NameGroup)
                .Select(x => x.edryofop_Data)
                .FirstOrDefaultAsync() ?? "default_edryofop_Data";

            var relatedDataBanckAccount = await _context.D1
                .Where(p => p.NameGroup == NameGroup)
                .Select(x => x.BanckAccount)
                .FirstOrDefaultAsync() ?? "default_BanckAccount";

            var relatedDataaddress = await _context.D1
                .Where(p => p.NameGroup == NameGroup)
                .Select(x => x.address)
                .FirstOrDefaultAsync() ?? "default_address_p";

            var relatedDataarea = await _context.D2
                .Where(p => p.NumberGroup == NumberGroup)
                .Select(x => x.area)
                .FirstOrDefaultAsync();

            var relatedDataNameGroup = await _context.D2
                .Where(p => p.NumberGroup == NumberGroup)
                .Select(x => x.NameGroup)
                .FirstOrDefaultAsync() ?? "default_Name";

            var relatedDataPIBS = await _context.D2
                .Where(p => p.NumberGroup == NumberGroup)
                .Select(x => x.PIBS)
                .FirstOrDefaultAsync() ?? "default_PIBS";

            var address = await _context.D4
              .Where(p => p.NumberGroup == NumberGroup)
              .Select(x => x.address)
              .FirstOrDefaultAsync();

            Console.WriteLine("IS IT TOV: " + IsTov);
            if (IsTov)
            {
                try
                {
                    var Director = _context.D1
                       .Where(p => p.NameGroup == NameGroup)
                       .Select(x => x.Director)
                       .FirstOrDefault() ?? "default_Director";

                    var PIBSDirector = GetShortenedName(Director);

                    var dirPath = CreateDirectoryForDoc($"{NumberGroup}-{NameGroup}");
                    string Aktdate = _context.D4.Where(p => p.NameGroup == NameGroup).Select(x => x.AktDate.ToString("dd/MM/yyyy")).FirstOrDefault();
                    //"C:\\work\\wwwroot\\word\\4_Акт_повернення прим_ФОП.docx"
                    var path_dog = await wordDoc.CopyOriginalFile(LoadSettingSublease("sublease-return-act-tov", "sublease-tov"), $"{RenameGog(DogovirSuborendu)}-{relatedDataNameGroup}-повернення-ТОВ", dirPath);
                    
                        var a =WordHelper.AddTextToContentControl(path_dog,
                                               DogovirSuborendu,
                                               DateTime.ToString("dd/MM/yyyy"),
                                               relatedDataFullname,
                                               relatedDataedryofop_Data,
                                               relatedDataList.ToString(),
                                               relatedDataarea.ToString(),
                                               address,
                                               //StrokDii,
                                               EndAktDate.ToString("dd/MM/yyyy"),
                                               //Num_To_Text.TestDataStrokdii(Convert.ToDateTime(DateTime)).ToString("dd/MM/yyyy"),
                                               relatedDataaddress,
                                               null,
                                               relatedDataBanckAccount,
                                               relatedDataPIBS,
                                               Num_To_Text.NumberToText(Convert.ToDouble(relatedDataarea)),
                                               Num_To_Text.SumToText(Convert.ToDouble(Suma)),
                                               Doublezero(Convert.ToDouble(Suma)),
                                               Num_To_Text.NumberMonthToText(EndAktDate.ToString("dd/MM/yyyy")).ToString(),
                                               Director,
                                               PIBSDirector,
                                               null,
                            null,
                            null,
                            null,
                            null

                                           );
                    Console.WriteLine("Document generation successful.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error generating document: {ex.Message}");
                }

                return Ok(new { success = "Документ для суборенды (ФОП) успешно сгенерирован!" });
            }
            else
            {
                try
                {
                    var dirPath = CreateDirectoryForDoc($"{NumberGroup}-{NameGroup}");
                    string Aktdate = _context.D4.Where(p => p.NameGroup == NameGroup).Select(x => x.AktDate.ToString("dd/MM/yyyy")).FirstOrDefault();
                    //"C:\\work\\wwwroot\\word\\4_Акт_повернення прим_ФОП.docx"
                    var path_dog = await wordDoc.CopyOriginalFile(LoadSettingSublease("sublease-return-act", "sublease"), $"{RenameGog(DogovirSuborendu)}-{relatedDataNameGroup}-повернення", dirPath);

                        var a = WordHelper.AddTextToContentControl(path_dog,
                                               DogovirSuborendu,
                                               DateTime.ToString("dd/MM/yyyy"),
                                               relatedDataFullname,
                                               relatedDataedryofop_Data,
                                               relatedDataList.ToString(),
                                               relatedDataarea.ToString(),
                                               address,
                                               //StrokDii,
                                               EndAktDate.ToString("dd/MM/yyyy"),
                                               //Num_To_Text.TestDataStrokdii(Convert.ToDateTime(DateTime)).ToString("dd/MM/yyyy"),
                                               relatedDataaddress,
                                               null,
                                               relatedDataBanckAccount,
                                               relatedDataPIBS,
                                               Num_To_Text.NumberToText(Convert.ToDouble(relatedDataarea)),
                                               Num_To_Text.SumToText(Convert.ToDouble(Suma)),
                                                Doublezero(Convert.ToDouble(Suma)),
                                               Num_To_Text.NumberMonthToText(EndAktDate.ToString("dd/MM/yyyy")).ToString(),
                                               null,
                                               null,
                                               null,
                            null,
                            null,
                            null,
                            null

                                           );
                    
                    Console.WriteLine("Document generation successful.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error generating document: {ex.Message}");
                }
                return Ok(new { success = "Документ для суборенды (ФОП) успешно сгенерирован!" });
            }
             
        }
    }
}
