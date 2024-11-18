using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SRMAgreement.Class;
using SRMAgreement.Data_Base;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace SRMAgreement.Controllers
{
    [Route("SaveFilesOnServer")]
    public class SaveFilesOnServerController : Controller
    {
        private readonly DataBaseContext _context;

        public SaveFilesOnServerController(DataBaseContext context)
        {
            _context = context;
        }

        private string SanitizeFileName(string input)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            return string.Concat(input.Select(c => invalidChars.Contains(c) ? '_' : c));
        }

        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IList<IFormFile> files,
            [FromForm] int numberGroup,
            [FromForm] string nameGroup,
            [FromForm] string ohronnaComp,
            [FromForm] string NumDog,
            [FromForm] string NumDog2,
            [FromForm] string StrokDii,
            [FromForm] string StrokDii2,
            [FromForm] string ResPerson,
            [FromForm] string Phone,
            [FromForm] string address)
        {
            if (files == null || !files.Any())
            {
                return BadRequest("No files were uploaded.");
            }

            if (numberGroup <= 0)
            {
                return BadRequest("Number group is required and must be greater than zero.");
            }

            var data = await _context.D2
                                     .Where(x => x.NumberGroup == numberGroup)
                                     .Select(x => x.NameGroup)
                                     .FirstOrDefaultAsync();

            if (data == null)
            {
                var _5DSave = new _5D
                {
                    NumberGroup = numberGroup,
                    NameGroup = nameGroup,
                    address = address,
                    OhronnaComp = ohronnaComp,
                    NumDog = NumDog,
                    NumDog2 = NumDog2,
                    StrokDii = Convert.ToDateTime(StrokDii),
                    StrokDii2 = Convert.ToDateTime(StrokDii2),
                    ResPerson = ResPerson,
                    Phone = Phone
                };
                _context.D5.AddRange(_5DSave);
                await _context.SaveChangesAsync();
            }

            var nameFolder = WordController.CreateDirectoryForDocGuard($"{numberGroup}-{data}");

            var existingD5 = await _context.D5.FirstOrDefaultAsync(d => d.NumberGroup == numberGroup);

            if (existingD5 == null)
            {
                var newD5Record = new _5D
                {
                    NumberGroup = numberGroup,
                    NameGroup = nameGroup,
                    address = address,
                    OhronnaComp = ohronnaComp,
                    NumDog = NumDog,
                    NumDog2 = NumDog2,
                    StrokDii = Convert.ToDateTime(StrokDii),
                    StrokDii2 = Convert.ToDateTime(StrokDii2),
                    ResPerson = ResPerson,
                    Phone = Phone
                };

                _context.D5.Add(newD5Record);
                await _context.SaveChangesAsync();

                existingD5 = newD5Record;
            }
            else
            {
                existingD5.NumberGroup = numberGroup;
                existingD5.NameGroup = nameGroup;
                existingD5.address = address;
                existingD5.OhronnaComp = ohronnaComp;
                existingD5.NumDog = NumDog;
                existingD5.NumDog2 = NumDog2;
                existingD5.StrokDii = Convert.ToDateTime(StrokDii);
                existingD5.StrokDii2 = Convert.ToDateTime(StrokDii2);
                existingD5.ResPerson = ResPerson;
                existingD5.Phone = Phone;

                _context.D5.Update(existingD5);
                await _context.SaveChangesAsync();
            }

            foreach (var file in files)
            {
                try
                {
                    var uniqueFileName =file.FileName;
                    var filePath = $"{nameFolder}\\{uniqueFileName}";

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    var newRecord = new PathToFilesGuard
                    {
                        _5dId = existingD5.Id,
                        PathTOServerFiles = filePath
                    };

                    _context.pathToFilesGuard.Add(newRecord);
                }
                catch (Exception ex)
                {
                    return BadRequest($"An error occurred while uploading the file: {ex.Message}");
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Database error: {ex.InnerException?.Message ?? ex.Message}");
            }

            return Ok(new { status = true, message = "Files uploaded successfully." });
        }

        [HttpPost("UploadOnlyFile")]
        public async Task<IActionResult> UploadOnlyFile(IList<IFormFile> files, [FromForm] int numberGroup)
        {
            Console.WriteLine("NUMBERGROUP----------------------------------------<>" + numberGroup);
            Console.WriteLine("FILECOUNT----------------------------------------<>" + files.Count);

            if (files == null || !files.Any())
            {
                return BadRequest("No files were uploaded.");
            }

            var data = await _context.D2
                                     .Where(x => x.NumberGroup == numberGroup)
                                     .Select(x => x.NameGroup)
                                     .FirstOrDefaultAsync();

            if (data == null)
            {
                return BadRequest("No record found in D2 for the given NumberGroup.");
            }

            var nameFolder = WordController.CreateDirectoryForSubleasePDF($"{numberGroup}-{data}");
            var existingD4 = await _context.D4.FirstOrDefaultAsync(d => d.NumberGroup == numberGroup);

            // Если запись D4 не найдена, вернем ошибку
            if (existingD4 == null)
            {
                return BadRequest("No record found in D4 for the given NumberGroup.");
            }

            foreach (var file in files)
            {
                try
                {
                    var uniqueFileName = file.FileName;
                    var filePath = $"{nameFolder}\\{uniqueFileName}";

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    var newRecord = new PdfFilePath_Sublease
                    {
                        _4DId = existingD4.Id,
                        PathToPdfFile_Sublease = filePath
                    };

                    _context.PdfFilePath_Sublease.Add(newRecord);
                }
                catch (Exception ex)
                {
                    return BadRequest($"An error occurred while uploading the file: {ex.Message}");
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Database error: {ex.InnerException?.Message ?? ex.Message}");
            }

            return Ok(new { status = true, message = "Files uploaded successfully." });
        }

    }
}
