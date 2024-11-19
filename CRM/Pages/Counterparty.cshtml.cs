using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SRMAgreement.Class;
using SRMAgreement.Data_Base;
using SRMAgreement.SuppCode;

namespace SRMAgreement.Pages
{
    [Authorize]
    public class CounterpartyModel : PageModel
    {
        private readonly DataBaseContext _context;
       
        public CounterpartyModel(DataBaseContext context)
        {
            _context = context;
            D1 = new List<_1D>();
            DataFromDB = new List<string>();
        }

        [BindProperty]
        public List<_1D> D1 { get; set; }
        public List<string> DataFromDB { get; private set; }
        public List<string> NumberGroupValues { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }

        public async Task OnGetAsync()
        {
            D1 = await _context.D1.ToListAsync();
            DataFromDB = await _context.D2.Select(x => x.NameGroup).ToListAsync();
            NumberGroupValues = _context.D2.GroupBy(x => x.NameGroup)
           .Where(g => g.Count() > 0)
            .Select(g => g.Key)
            .ToList();
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {

            if (action == "AddNewRow")
            {
                DataFromDB = await _context.D2.Select(x => x.NameGroup).ToListAsync();
                NumberGroupValues = _context.D2.GroupBy(x => x.NameGroup)
                .Where(g => g.Count() > 0)
                .Select(g => g.Key)
                .ToList();
                var newRow = new _1D();
                D1.Add(newRow);
            }
            else if (action == "SaveChangesWithNewRows")
            {
                foreach (var item in D1)
                {
                    if (item.Id == 0)
                    {
                        _context.D1.Add(item);
                    }
                    else
                    {
                        var entity = await _context.D1.FindAsync(item.Id);
                        if (entity != null)
                        {
                            entity.NameGroup = item.NameGroup;
                            entity.Fullname = DeleteSpace.Deletespace(item.Fullname);
                            entity.rnokpp = DeleteSpace.Deletespace(item.rnokpp);
                            entity.address = DeleteSpace.Deletespace(item.address);
                            entity.edryofop_Data = DeleteSpace.Deletespace(item.edryofop_Data);
                            entity.BanckAccount = DeleteSpace.Deletespace(item.BanckAccount);
                            entity.Director = DeleteSpace.Deletespace(item.Director);
                            entity.ResPerson = item.ResPerson;
                            entity.Phone = item.Phone;
                            entity.Email = item.Email;
                            entity.Tov = item.Tov;
                            Console.WriteLine($"Setting Tov: {entity.Tov} for Id: {entity.Id}");
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
            D1.Clear();
            DataFromDB = await _context.D2.Select(x => x.NameGroup).ToListAsync();
            NumberGroupValues = _context.D2.GroupBy(x => x.NameGroup)
               .Where(g => g.Count() > 0)
               .Select(g => g.Key)
               .ToList();
            string searchQueryLower = ToUpperFirstLetter(SearchQuery);
            D1 = await _context.D1
                .Where(e => e.NameGroup.Contains(searchQueryLower) || e.NameGroup == ToUpperAll(SearchQuery) || e.NameGroup == ToLowerAll(SearchQuery) || e.NameGroup == ToUpperAllLaters(SearchQuery) ||
                            e.Fullname.Contains(searchQueryLower) ||
                            e.rnokpp.ToString().Contains(SearchQuery) ||
                            e.address.Contains(searchQueryLower) ||
                            e.edryofop_Data.Contains(searchQueryLower) ||
                            e.BanckAccount.Contains(searchQueryLower) ||
                            e.Director.Contains(searchQueryLower) ||
                            e.ResPerson.Contains(searchQueryLower) ||
                            e.Phone.Contains(SearchQuery) ||
                            e.Email.Contains(SearchQuery)
                            )
                .ToListAsync();
            return Page();
        }

    }
}
