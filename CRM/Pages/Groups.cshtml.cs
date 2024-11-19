using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SRMAgreement.Class;
using SRMAgreement.Data_Base;
using System.Linq;
using SRMAgreement.SuppCode;

namespace SRMAgreement.Pages
{
    [Authorize]
    public class GroupsModel : PageModel
    {
        private readonly DataBaseContext _context;
        public GroupsModel(DataBaseContext context)
        {
            _context = context;
            D2 = new List<_2D>();
        }

        [BindProperty]
        public List<_2D> D2 { get; set; }

        [BindProperty]
        public List<SubleaseDop> Testview { get; set; } = new List<SubleaseDop>();

        [BindProperty]
        public SubleaseDop TestviewTest { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }

        public async Task OnGetAsync()
        {
            D2 = await _context.D2
             .OrderBy(d => d.NumberGroup)
             .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            if (action == "AddNewRow")
            {
                var newRow = new _2D();
                D2.Insert(0, newRow);
                return Page();
            }
            else if (action == "SaveChangesWithNewRows")
            {
                foreach (var item in D2)
                {
                    string addressNS = DeleteSpace.Deletespace(item.address);
                    if (item.Id == 0)
                    {
                        _context.D2.Add(item);
                    }
                    else
                    {
                        var entity = await _context.D2.FindAsync(item.Id);
                        if (entity != null )
                        {
                            entity.NumberGroup = item.NumberGroup;
                            entity.NameGroup = item.NameGroup;
                            entity.PIBS = DeleteSpace.Deletespace(item.PIBS);
                            entity.address = addressNS;
                            entity.area = item.area;
                            entity.rent = item.rent;
                            entity.PIBS = item.PIBS;
                            entity.isAlert = item.isAlert;
                            entity.DateCloseDepartment = item.DateCloseDepartment;
                        }
                        else 
                        {
                            return BadRequest();
                        }
                    }
                }
                await _context.SaveChangesAsync();
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
            string searchQueryLower = ToUpperFirstLetter(SearchQuery);
            D2.Clear();
            D2 = await _context.D2
                .Where(e =>
                    e.NumberGroup.ToString().Contains(SearchQuery) ||
                    e.NameGroup.Contains(searchQueryLower) || e.NameGroup == ToUpperAll(SearchQuery) || e.NameGroup == ToLowerAll(SearchQuery) || e.NameGroup == ToUpperAllLaters(SearchQuery) ||
                    e.PIBS.Contains(searchQueryLower) || e.PIBS.Contains(SearchQuery) || e.PIBS.Contains(ToUpperAll(SearchQuery)) ||
                    e.address.Contains(searchQueryLower) || e.address.Contains(SearchQuery) || e.address.Contains(ToUpperAll(SearchQuery)) ||
                    e.area.ToString().Contains(SearchQuery)
                ).OrderBy(e => e.NumberGroup)
                .ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostLoadDataModelAsync(int numgroup)
        {
            var a = await _context.SubleaseDop.FirstOrDefaultAsync(x => x.NumberGroup == numgroup);
            TestviewTest = a ?? new SubleaseDop();
            D2 = await _context.D2
            .OrderBy(d => d.NumberGroup)
            .ToListAsync();
            return Page();
        }
    }
}