using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRMAgreement.Class;
using SRMAgreement.Data_Base;
using System.IO;
using System.Threading.Tasks;

namespace SRMAgreement.Controllers
{
    [Route("LoadPDFController")]
    public class LoadPDFController : Controller
    {
        private readonly DataBaseContext _context;

        public LoadPDFController(DataBaseContext context)
        {
            _context = context;
        }

        [HttpGet("GetDocument")]
        public async Task<IActionResult> GetDocument(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return BadRequest(new { message = "File path is required." });
            }

            Console.WriteLine($"FILE PATH TO PHYSICALPATH: {filePath}");
            Console.WriteLine($"FILE PATH GETFILENAME: {Path.GetFileName(filePath)}");

            if (!System.IO.File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return NotFound(new { message = "File not found on server." });
            }

            var contentType = "application/pdf";
            return PhysicalFile(filePath, contentType, Path.GetFileName(filePath));
        }

        [HttpGet("GetDocumentById")]
        public async Task<IActionResult> GetDocumentById(int id)
        {
            if (id == 0)
            {
                return BadRequest(new { message = "File ID is required." });
            }

            var GetpathToFile = await _context.D5
                .Include(d => d.PathToFilesGuard)
                .Where(x => x.Id == id)
                .SelectMany(x => x.PathToFilesGuard)
                .ToListAsync();

            if (!GetpathToFile.Any())
            {
                return NotFound(new { message = "No files found for the specified ID." });
            }

            // Список URL-адресов для клиентской стороны
            var fileUrls = new List<string>();

            foreach (var file in GetpathToFile)
            {
                var filePath = file.PathTOServerFiles;

                if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
                {
                    Console.WriteLine($"File not found: {filePath}");
                    continue; // Пропускаем файлы, которых нет на сервере
                }

                // Создаём URL для каждого файла, чтобы его можно было открыть на фронтенде
                var fileUrl = Url.Action("GetFile", "LoadPDF", new { filePath });
                fileUrls.Add(fileUrl);
            }

            // Возвращаем список URL-адресов
            return Ok(fileUrls);
        }

        [HttpGet("GetFile")]
        public IActionResult GetFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            {
                return NotFound(new { message = "File not found on server." });
            }

            var contentType = "application/pdf";
            return PhysicalFile(filePath, contentType, Path.GetFileName(filePath));
        }


        [HttpDelete("DeleteDocument")]
        public async Task<IActionResult> DeleteDocument(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            var fileRecord = await _context.pathToFilesGuard
                .FirstOrDefaultAsync(x => x.PathTOServerFiles == filePath);

            if (fileRecord != null)
            {
                _context.pathToFilesGuard.Remove(fileRecord);
                await _context.SaveChangesAsync();
            }
            else
            {
                return NotFound("Запис не знайдено в базі даних.");
            }
            return Ok("Файл та запис у базі даних успішно видалено.");
        }

        [HttpDelete("DeleteDocumentSublease")]
        public async Task<IActionResult> DeleteDocumentSublease(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            var fileRecord = await _context.PdfFilePath_Sublease
                .FirstOrDefaultAsync(x => x.PathToPdfFile_Sublease == filePath);

            if (fileRecord != null)
            {
                _context.PdfFilePath_Sublease.Remove(fileRecord);
                await _context.SaveChangesAsync();
            }
            else
            {
                return NotFound("Запис не знайдено в базі даних.");
            }
            return Ok("Файл та запис у базі даних успішно видалено.");
        }

        [HttpGet("GetFilesById")]
        public async Task<IActionResult> GetFilesById(int Id)
        {
            var PathToFiles = await _context.PdfFilePath_Sublease
                .Where(x => x._4DId == Id)
                .Select(x => new
                {
                    FileName = x.PathToPdfFile_Sublease.Substring(x.PathToPdfFile_Sublease.LastIndexOf('/') + 1),
                    PhysicalPath = x.PathToPdfFile_Sublease
                })
                .ToListAsync();

            return Ok(PathToFiles);
        }

    }
}
