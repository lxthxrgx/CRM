using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SRMAgreement.Class;
using SRMAgreement.Data_Base;
using System.Collections;
using System.Collections.Generic;

namespace SRMAgreement.Pages
{
    public class BookkeepingModel : PageModel
    {
        private readonly DataBaseContext _context;
        public BookkeepingModel(DataBaseContext context)
        {
            _context = context;
            D4List = new List<_4DBook>();
            Guard = new List<_5D>();
            Counterparty = new List<_1D>();
        }
        [BindProperty]
        public List<_1D> Counterparty { get; set; }
        public List<_4DBook> D4List { get; set; }
        public List<_4DBook> D4List_Num { get; set; }
        public List<_5D> Guard { get; set; }

        [BindProperty]
        public FilterDataModel FilterData { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }

        public async Task OnGetAsync()
        {
            var d4List = await _context.D4Bookk
                .OrderByDescending(d4 => d4.Updatet_Record)
                .ToListAsync();

            D4List.AddRange(d4List);

            var numberGroups = d4List.Select(d4 => d4.NumberGroup).Distinct().ToList();
            var nameGroups = d4List.Select(d4 => d4.NameGroup).Distinct().ToList();

            var ListGuard = await _context.D5
                .Where(d5 => numberGroups.Contains(d5.NumberGroup))
                .ToListAsync();

            Guard.AddRange(ListGuard);

            Counterparty = await _context.D1
                .Where(d1 => nameGroups.Contains(d1.NameGroup)).
                ToListAsync();
        }
        public async Task<IActionResult> OnPostSearchQueryAsync()
        {

            Console.WriteLine("SEARCHQUERY---------------------------------<>" + SearchQuery);
            if (string.IsNullOrEmpty(SearchQuery))
            {
                return Page();
            }
            D4List.Clear();
            Guard.Clear();
            Counterparty.Clear();

            string searchQueryLower = ToUpperFirstLetter(SearchQuery);

            D4List = await _context.D4Bookk
                .Where(e => e.NumberGroup.ToString().Contains(SearchQuery) ||
                            e.NameGroup.Contains(searchQueryLower) ||
                            e.address.Contains(searchQueryLower) ||
                            e.DogovirSuborendu.Contains(SearchQuery) ||
                            e.DateTime.ToString().Contains(SearchQuery) ||
                            e.EndAktDate.ToString().Contains(SearchQuery) ||
                            e.Suma.Contains(SearchQuery) ||
                            e.AktDate.ToString().Contains(SearchQuery))
                .OrderBy(e => e.NumberGroup)
                .ToListAsync();

            var numberGroups = D4List.Select(d4 => d4.NumberGroup).Distinct().ToList();
            var nameGroups = D4List.Select(d4 => d4.NameGroup).Distinct().ToList();

            Guard = await _context.D5
                .Where(x => numberGroups.Contains(x.NumberGroup))
                .ToListAsync();

            Counterparty = await _context.D1.Where(x => nameGroups.Contains(x.NameGroup)).ToListAsync();


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

        public async Task<IActionResult> OnPostGetDataAsync(FilterDataModel filterData)
        {
            Console.WriteLine("Groups--------------------->" + filterData.Groups);
            Console.WriteLine("StartDate--------------------->" + filterData.StartDate);
            Console.WriteLine("EndDate--------------------->" + filterData.EndDate);

            var query = _context.D4Bookk.AsQueryable();

            if (filterData.Groups != null && filterData.Groups.Any())
            {
                query = query.Where(item => filterData.Groups.Contains(item.NameGroup));
            }

            if (!string.IsNullOrWhiteSpace(filterData.StartDate) && DateTime.TryParse(filterData.StartDate, out DateTime startDate))
            {
                query = query.Where(item => item.Updatet_Record >= startDate);
            }

            if (!string.IsNullOrWhiteSpace(filterData.EndDate) && DateTime.TryParse(filterData.EndDate, out DateTime endDate))
            {
                query = query.Where(item => item.Updatet_Record <= endDate);
            }

            var result = await query.OrderByDescending(d4 => d4.Updatet_Record).ToListAsync();

            D4List = result;
            var numberGroups = result.Select(d4 => d4.NumberGroup).Distinct().ToList();
            var nameGroups = result.Select(d4 => d4.NameGroup).Distinct().ToList();

            Guard = await _context.D5.Where(d5 => numberGroups.Contains(d5.NumberGroup)).ToListAsync();
            Counterparty = await _context.D1.Where(d1 => nameGroups.Contains(d1.NameGroup)).ToListAsync();

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
