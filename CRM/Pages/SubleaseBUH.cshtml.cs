using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SRMAgreement.Class;
using SRMAgreement.Data_Base;
using SRMAgreement.SuppCode;

namespace SRMAgreement.Pages
{
    public class SubleaseBUHModel : PageModel
    {
        private readonly DataBaseContext _context;
        private const int PageSize = 50;
        public SubleaseBUHModel(DataBaseContext context)
        {
            _context = context;
            D4 = new List<_4DBook>();
            D1 = new List<string>();
        }

        [BindProperty]
        public List<_4DBook> D4 { get; set; }
        public List<string> D1 { get; set; }
        public List<int> D2NumberGroup { get; set; }
        public List<int> NumberGroupValues { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }

        public Dictionary<int, PageRangeSubleaseBuh> PageRanges { get; set; } = new Dictionary<int, PageRangeSubleaseBuh>();

        public async Task OnGetAsync()
        {
            D1 = await _context.D1.Select(x => x.Fullname).ToListAsync();

            int totalRecords = await _context.D4Bookk.CountAsync();
            TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);

            PageRanges.Clear();
            for (int page = 1; page <= TotalPages; page++)
            {
                var pageData = await _context.D4Bookk
                                            .OrderBy(d => d.NumberGroup)
                                            .Skip((page - 1) * PageSize)
                                            .Take(PageSize)
                                            .ToListAsync();

                if (pageData.Any())
                {
                    PageRanges[page] = new PageRangeSubleaseBuh
                    {
                        MinNumberGroup = pageData.First().NumberGroup,
                        MaxNumberGroup = pageData.Last().NumberGroup
                    };
                }
                else
                {
                    PageRanges[page] = new PageRangeSubleaseBuh
                    {
                        MinNumberGroup = 0,
                        MaxNumberGroup = 0
                    };
                }
            }

            D4 = await _context.D4Bookk
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

                var newRow = new _4DBook();

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
                            var newEntity = new _4DBook
                            {
                                NumberGroup = item.NumberGroup,
                                NameGroup = DeleteSpace.Deletespace(item.NameGroup),
                                address = DeleteSpace.Deletespace(item.address),
                                DogovirSuborendu = DeleteSpace.Deletespace(item.DogovirSuborendu),
                                DateTime = item.DateTime,
                                EndAktDate = item.EndAktDate,
                                Suma = DeleteSpace.Deletespace(item.Suma),
                                Suma2 = DeleteSpace.Deletespace(item.Suma2),
                                AktDate = item.AktDate,
                                payments_term = item.payments_term
                            };

                            await _context.D4Bookk.AddAsync(newEntity);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            var entity = await _context.D4Bookk.FindAsync(item.Id);
                            if (entity != null)
                            {
                                entity.NumberGroup = item.NumberGroup;
                                entity.NameGroup = DeleteSpace.Deletespace(item.NameGroup);
                                entity.address = DeleteSpace.Deletespace(item.address);
                                entity.DogovirSuborendu = DeleteSpace.Deletespace(item.DogovirSuborendu);
                                entity.DateTime = item.DateTime;
                                entity.EndAktDate = item.EndAktDate;
                                entity.Suma = DeleteSpace.Deletespace(item.Suma);
                                entity.Suma2 = DeleteSpace.Deletespace(item.Suma2);
                                entity.AktDate = item.AktDate;
                                entity.payments_term = item.payments_term;

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
            D4 = await _context.D4Bookk
                .Where(e => e.NumberGroup.ToString() == SearchQuery ||
                      e.NameGroup.Contains(searchQueryLower) ||
                            e.address.Contains(searchQueryLower) ||
                            e.DogovirSuborendu.Contains(SearchQuery) ||
                            e.DateTime.ToString().Contains(SearchQuery) ||
                            e.EndAktDate.ToString().Contains(SearchQuery) ||
                            e.Suma.Contains(SearchQuery) ||
                            e.payments_term.ToString().Contains(SearchQuery) ||
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
            D4 = _context.D4Bookk.Where(p => p.NumberGroup == number).ToList();
        }
    }

    public class PageRangeSubleaseBuh
    {
        public int MinNumberGroup { get; set; }
        public int MaxNumberGroup { get; set; }
    }
}
