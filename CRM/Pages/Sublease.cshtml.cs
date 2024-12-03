using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SRMAgreement.Class;
using SRMAgreement.Data_Base;
using SRMAgreement.SuppCode;
using System.Linq;

namespace SRMAgreement.Pages
{
    [Authorize]
    public class SubleaseModel : PageModel
    {
        private readonly DataBaseContext _context;
        private readonly DataBaseArchive _archive;
        private const int PageSize = 50;
        public SubleaseModel(DataBaseContext context, DataBaseArchive archive)
        {
            _context = context;
            D4 = new List<_4D>();
            D1 = new List<string>();
            _archive = archive;
        }

        [BindProperty]
        public List<_4D> D4 { get; set; }
        public List<string> D1 { get; set; }
        public List<int> D2NumberGroup { get; set; }
        public List<int> NumberGroupValues { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }

        public Dictionary<int, PageRange> PageRanges { get; set; } = new Dictionary<int, PageRange>();

        public async Task OnGetAsync()
        {
            D1 = await _context.D1.Select(x => x.Fullname).ToListAsync();

            int totalRecords = await _context.D4.CountAsync();
            TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);

            PageRanges.Clear();
            for (int page = 1; page <= TotalPages; page++)
            {
                var pageData = await _context.D4.Include(d => d.PathToPdfFiles_Sublease)
                                            .OrderBy(d => d.NumberGroup)
                                            .Skip((page - 1) * PageSize)
                                            .Take(PageSize)
                                            .ToListAsync();

                if (pageData.Any())
                {
                    PageRanges[page] = new PageRange
                    {
                        MinNumberGroup = pageData.First().NumberGroup,
                        MaxNumberGroup = pageData.Last().NumberGroup
                    };
                }
                else
                {
                    PageRanges[page] = new PageRange
                    {
                        MinNumberGroup = 0,
                        MaxNumberGroup = 0
                    };
                }
            }

            D4 = await _context.D4.Include(d => d.PathToPdfFiles_Sublease)
                .OrderBy(d => d.NumberGroup)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var ifAlert = await _context.D2.Where(d => d.isAlert == true).ToListAsync();

            D2NumberGroup = ifAlert.Where(d => d.isAlert == true).Select(d => d.NumberGroup).ToList();

            NumberGroupValues = NumberGroupValues = _context.D2
                .OrderBy(d => d.NumberGroup)
                .Select(d => d.NumberGroup)
                .ToList();

            var repeatingGroups = await _context.D4
                .GroupBy(x => x.NumberGroup)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToListAsync();

            var filteredByDate = await _context.D4
                .Where(x => repeatingGroups.Contains(x.NumberGroup) && x.EndAktDate <= DateTime.Today)
                .OrderBy(x => x.NumberGroup)
                .ThenBy(x => x.EndAktDate)
                .ToListAsync();

            var finalFilteredRecords = filteredByDate
                .GroupBy(x => x.NumberGroup)
                .Select(g => g.OrderBy(x => x.EndAktDate).FirstOrDefault())
                .Where(x => x.Done == true || x.Done == Convert.ToBoolean(1) || x.Done != null && x.EndAktDate <= DateTime.Today)
                .ToList();

            var archiveList = finalFilteredRecords.Select(x => new Archive_4D
            {
                NumberGroup = x.NumberGroup,
                NameGroup = x.NameGroup,
                address = x.address,
                DogovirSuborendu = x.DogovirSuborendu,
                DateTime = x.DateTime,
                EndAktDate = x.EndAktDate,
                Suma = x.Suma,
                Suma2 = x.Suma2,
                AktDate = x.AktDate,
                Done = x.Done
            }).ToList();


            if (archiveList.Any())
            {
                foreach (var a in finalFilteredRecords)
                {
                    Console.WriteLine(a.NumberGroup + "+" + a.EndAktDate);
                }

                _archive.Archive_4D.AddRange(archiveList);
                await _archive.SaveChangesAsync();

                _context.D4.RemoveRange(finalFilteredRecords);
                await _context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("Нет данных для добавления в архив.");
            }
        }

        public async Task<IActionResult> OnPostAsync(string action, int? rowIndex)
        {
            if (action == "AddNewRow")
            {
                D1 = await _context.D1.Select(x => x.Fullname).ToListAsync();
                NumberGroupValues = _context.D2
                .OrderBy(d => d.NumberGroup)
                .Select(d => d.NumberGroup)
                .ToList();

                var ifAlert = await _context.D2.Where(d => d.isAlert == true).ToListAsync();
                D2NumberGroup = ifAlert.Where(d => d.isAlert == true).Select(d => d.NumberGroup).ToList();

                var newRow = new _4D();

                D4.Insert(0, newRow);
                return Page();
            }

            else if (action == "SaveChangesWithNewRows")
            {

                try
                {
                    foreach (var item in D4)
                    {
                        if (item.Id == 0)
                        {
                            var newEntity = new _4D
                            {
                                NumberGroup = item.NumberGroup,
                                NameGroup = DeleteSpace.Deletespace(item.NameGroup),
                                address = DeleteSpace.Deletespace(item.address),
                                DogovirSuborendu = DeleteSpace.Deletespace(item.DogovirSuborendu),
                                DateTime = item.DateTime,
                                EndAktDate = item.EndAktDate,
                                Suma = DeleteSpace.Deletespace(item.Suma),
                                Suma2 = DeleteSpace.Deletespace(item.Suma2),
                                AktDate = item.AktDate
                            };

                            await _context.D4.AddAsync(newEntity);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            var entity = await _context.D4.FindAsync(item.Id);
                            if (entity != null)
                            {
                                if (entity.NumberGroup != item.NumberGroup ||
                                    entity.NameGroup != item.NameGroup ||
                                    entity.address != item.address ||
                                    entity.DogovirSuborendu != item.DogovirSuborendu ||
                                    entity.DateTime != item.DateTime ||
                                    entity.EndAktDate != item.EndAktDate ||
                                    entity.Suma != item.Suma ||
                                    entity.Suma2 != item.Suma2 ||
                                    entity.AktDate != item.AktDate)
                                {
                                    var changeRecord = await _context.D4Bookk
                                        .FirstOrDefaultAsync(b => b.IdFromD4 == item.Id);

                                    if (changeRecord == null)
                                    {
                                        changeRecord = new _4DBook
                                        {
                                            IdFromD4 = item.Id,
                                            NumberGroup = item.NumberGroup,
                                            NameGroup = DeleteSpace.Deletespace(item.NameGroup),
                                            address = DeleteSpace.Deletespace(item.address),
                                            DogovirSuborendu = DeleteSpace.Deletespace(item.DogovirSuborendu),
                                            DateTime = item.DateTime,
                                            AktDate = Num_To_Text.TestDataStrokdii(item.AktDate),
                                            EndAktDate = item.EndAktDate,
                                            Suma = DeleteSpace.Deletespace(item.Suma),
                                            Suma2 = DeleteSpace.Deletespace(item.Suma2),
                                            Updatet_Record = DateTime.Now
                                        };

                                        _context.D4Bookk.Add(changeRecord);
                                        await _context.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        // Обновляем запись, если запись существует
                                        changeRecord.IdFromD4 = item.Id;
                                        changeRecord.NumberGroup = item.NumberGroup;
                                        changeRecord.NameGroup = DeleteSpace.Deletespace(item.NameGroup);
                                        changeRecord.address = DeleteSpace.Deletespace(item.address);
                                        changeRecord.DogovirSuborendu = DeleteSpace.Deletespace(item.DogovirSuborendu);
                                        changeRecord.DateTime = item.DateTime;
                                        changeRecord.AktDate = Num_To_Text.TestDataStrokdii(item.AktDate);
                                        changeRecord.EndAktDate = item.EndAktDate;
                                        changeRecord.Suma = DeleteSpace.Deletespace(item.Suma);
                                        changeRecord.Suma2 = DeleteSpace.Deletespace(item.Suma2);
                                        changeRecord.Updatet_Record = DateTime.Now;
                                        await _context.SaveChangesAsync();
                                    }
                                }
                                entity.NumberGroup = item.NumberGroup;
                                entity.NameGroup = DeleteSpace.Deletespace(item.NameGroup);
                                entity.address = DeleteSpace.Deletespace(item.address);
                                entity.DogovirSuborendu = DeleteSpace.Deletespace(item.DogovirSuborendu);
                                entity.DateTime = item.DateTime;
                                entity.EndAktDate = item.EndAktDate;
                                entity.Suma = DeleteSpace.Deletespace(item.Suma);
                                entity.Suma2 = DeleteSpace.Deletespace(item.Suma2);
                                entity.AktDate = item.AktDate;

                                await _context.SaveChangesAsync();
                            }

                        }

                    }
                    await _context.SaveChangesAsync();

                }
                catch (Exception ex)
                {

                    return BadRequest();
                }
                return RedirectToPage();
            }
            else if (action == "Search")
            {
                if (string.IsNullOrEmpty(SearchQuery))
                {
                    return RedirectToPage();
                }
                return await OnPostSearchAsync();
            }
            return Page();
        }

        public string ToUpperFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }
        public string ToUpperAll(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            return char.ToUpper(input[0]) + input.Substring(1);
        }

        public string ToUpperAllLaters(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            return input.ToUpper();
        }

        public string ToLowerAll(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            return input.ToLower();
        }

        public async Task<IActionResult> OnPostSearchAsync()
        {
            if (string.IsNullOrEmpty(SearchQuery))
            {
                return Page();
            }
            D4.Clear();
            NumberGroupValues = NumberGroupValues = _context.D2
                .OrderBy(d => d.NumberGroup)
                .Select(d => d.NumberGroup)
                .ToList();
            var ifAlert = await _context.D2.Where(d => d.isAlert == true).ToListAsync();
            D2NumberGroup = ifAlert.Where(d => d.isAlert == true).Select(d => d.NumberGroup).ToList();

            string searchQueryLower = ToUpperFirstLetter(SearchQuery);
            D4 = await _context.D4
                .Where(e => e.NumberGroup.ToString() == SearchQuery ||
                      e.NameGroup.Contains(searchQueryLower) || e.NameGroup == ToUpperAll(SearchQuery) || e.NameGroup == ToLowerAll(SearchQuery) || e.NameGroup == ToUpperAllLaters(SearchQuery) ||
                            e.address.Contains(searchQueryLower) ||
                            e.DogovirSuborendu.Contains(SearchQuery) ||
                            e.DateTime.ToString().Contains(SearchQuery) ||
                            e.EndAktDate.ToString().Contains(SearchQuery) ||
                            e.Suma.Contains(SearchQuery) ||
                            e.AktDate.ToString().Contains(SearchQuery))
                .OrderBy(e => e.NumberGroup)
                .ToListAsync();
            return Page();
        }
        public async Task OnGetByNumber(int number)
        {
            NumberGroupValues = NumberGroupValues = _context.D2
                .OrderBy(d => d.NumberGroup)
                .Select(d => d.NumberGroup)
                .ToList();
            D4 = _context.D4.Where(p => p.NumberGroup == number).ToList();
        }
    }

    public class PageRange
    {
        public int MinNumberGroup { get; set; }
        public int MaxNumberGroup { get; set; }
    }
}

