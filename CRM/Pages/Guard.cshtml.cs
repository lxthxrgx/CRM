using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SRMAgreement.Class;
using SRMAgreement.Controllers;
using SRMAgreement.Data_Base;
using static SRMAgreement.Pages.BookkeepingModel;

namespace SRMAgreement.Pages
{
    [Authorize]
    public class GuardModel : PageModel
    {
        private readonly DataBaseContext _context;

        public GuardModel(DataBaseContext context)
        {
            _context = context;
            D5 = new List<_5D>();
            D1 = new List<_1D>();
            DataFromDB = new List<_2D>();
            status = new List<Status>();
        }

        [BindProperty]
        public List<_5D> D5 { get; set; }
        public List<_1D> D1 { get; set; }
        public List<_2D> DataFromDB { get; private set; }
        public List<Status> status { get; private set; }
        public List<int> NumberGroupValues { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }
        public List<_5D> SearchResults { get; set; }


        [BindProperty]
        public FilterDataModel FilterData { get; set; }

        public async Task OnGetAsync()
        {
            D1 = await _context.D1.ToListAsync();
            status = await _context.status.ToListAsync();
            D5 = await _context.D5
           .Include(d => d.PathToFilesGuard).OrderBy(d => d.NumberGroup)
           .ToListAsync();
            DataFromDB = await _context.D2.ToListAsync();
            NumberGroupValues = await _context.D2
                .OrderBy(d => d.NumberGroup)
                .Select(d => d.NumberGroup)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(string action, List<IFormFile> fileUploads)
        {
            if (action == "SaveChanges")
            {
                foreach (var item in D5)
                {
                    var entity = await _context.D5.FindAsync(item.Id);
                    if (entity != null)
                    {
                        entity.NumberGroup = item.NumberGroup;
                        entity.NameGroup = item.NameGroup;
                        entity.address = item.address;
                        entity.OhronnaComp = item.OhronnaComp;
                        entity.NumDog = item.NumDog;
                        entity.NumDog2 = item.NumDog2;
                        entity.StrokDii = item.StrokDii;
                        entity.StrokDii2 = item.StrokDii2;
                        entity.ResPerson = item.ResPerson;
                        entity.Phone = item.Phone;
                        _context.Entry(entity).State = EntityState.Modified;
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToPage();
            }
            else if (action == "AddNewRow")
            {
                D1 = await _context.D1.ToListAsync();
                status = await _context.status.ToListAsync();
                DataFromDB = await _context.D2.ToListAsync();
                NumberGroupValues = await _context.D2
                    .OrderBy(d => d.NumberGroup)
                    .Select(d => d.NumberGroup)
                    .ToListAsync();
                D5 = await _context.D5.OrderBy(x => x.NumberGroup).ToListAsync();
                var newRow = new _5D();
                D5.Insert(0, newRow);
                return Page();
            }
            else if (action == "SaveChangesWithNewRows")
            {
                for (var i = 0; i < D5.Count; i++)
                {
                    var item = D5[i];
                    if (item.Id == 0)
                    {
                        _context.D5.Add(item);
                    }
                    else
                    {
                        var entity = await _context.D5.FindAsync(item.Id);
                        if (entity != null)
                        {
                            entity.NumberGroup = item.NumberGroup;
                            entity.NameGroup = item.NameGroup;
                            entity.address = item.address;
                            entity.OhronnaComp = item.OhronnaComp;
                            entity.NumDog = item.NumDog;
                            entity.NumDog2 = item.NumDog2;
                            entity.StrokDii = item.StrokDii;
                            entity.StrokDii2 = item.StrokDii2;
                            entity.ResPerson = item.ResPerson;
                            entity.Phone = item.Phone;
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



        private async Task<IActionResult> OnPostSearchAsync()
        {
            if (string.IsNullOrEmpty(SearchQuery))
            {
                return Page();
            }

            D1 = await _context.D1.ToListAsync();
            status = await _context.status.ToListAsync();
            DataFromDB = await _context.D2.ToListAsync();
            NumberGroupValues = _context.D2.Select(d => d.NumberGroup).ToList();

            D5 = await _context.D5
                .Where(e => e.NumberGroup.ToString() == SearchQuery ||
                    e.address.Contains(SearchQuery) ||
                            e.OhronnaComp.Contains(SearchQuery) ||
                             e.NumDog.ToString().Contains(SearchQuery) ||
                             e.StrokDii.ToString().Contains(SearchQuery) ||
                            e.ResPerson.ToString().Contains(SearchQuery) ||
                            e.Phone.Contains(SearchQuery)
                            ).Include(d => d.PathToFilesGuard).OrderBy(d => d.NumberGroup)
                .ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostGetDataAsync(FilterDataModel filterData)
        {
            Console.WriteLine("Groups--------------------->" + filterData.Groups);
            Console.WriteLine("StartDate--------------------->" + filterData.StartDate);
            Console.WriteLine("EndDate--------------------->" + filterData.EndDate);

            var query = _context.D5.AsQueryable();

            if (filterData.Groups != null && filterData.Groups.Any())
            {
                query = query.Where(item => filterData.Groups.Contains(item.NameGroup));
            }

            //if (!string.IsNullOrWhiteSpace(filterData.StartDate) && DateTime.TryParse(filterData.StartDate, out DateTime startDate))
            //{
            //    query = query.Where(item => item.Updatet_Record >= startDate);
            //}

            //if (!string.IsNullOrWhiteSpace(filterData.EndDate) && DateTime.TryParse(filterData.EndDate, out DateTime endDate))
            //{
            //    query = query.Where(item => item.Updatet_Record <= endDate);
            //}

            var result = await query.OrderBy(d5 => d5.NumberGroup).ToListAsync();

            D5 = result;

            D1 = await _context.D1.ToListAsync();
            status = await _context.status.ToListAsync();
            DataFromDB = await _context.D2.ToListAsync();
            NumberGroupValues = _context.D2.Select(d => d.NumberGroup).ToList();

            return Page();
        }

        public class FilterDataModel
        {
            public List<string> Groups { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
        }
    }
}
