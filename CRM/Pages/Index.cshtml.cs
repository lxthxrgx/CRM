using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SRMAgreement.Class;
using SRMAgreement.Data_Base;

namespace SRMAgreement.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly DataBaseContext _context;
        public IndexModel(ILogger<IndexModel> logger, DataBaseContext context)
        {
            _context = context;
            _logger = logger;
            D3 = new List<_3D>();
            D4 = new List<_4D>();
            D5 = new List<_5D>();
        }

        [BindProperty]
        public List<_3D> D3 { get; set; }
        public List<_4D> D4 { get; set; }
        public List<_5D> D5 { get; private set; }

        public async Task OnGet()
        {
            DateTime today = DateTime.UtcNow.Date;
            DateTime oneMonthLater = today.AddMonths(1);
            D4.Clear();
            ViewData["Today"] = today;
            ViewData["OneMonthLater"] = oneMonthLater;

            var D4_filtered = await _context.D4
                                          .Where(p => (p.EndAktDate >= today && p.EndAktDate <= oneMonthLater))
                                          .ToListAsync();

            D4.AddRange(D4_filtered.Where(a =>
                (a.Done == null || a.Done == Convert.ToBoolean(0) || a.Done == false) ||
                (a.Done == true || a.Done == Convert.ToBoolean(1) && a.EndAktDate >= today && a.EndAktDate <= oneMonthLater)
                ));
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await HttpContext.SignOutAsync();
            return RedirectToPage("/Index");
        }
        
            public static string TestDataStan(DateTime date)
            {
                var StringToDate = Convert.ToDateTime(date);
                int dayNumber = StringToDate.Day;
                int monthNumber = StringToDate.Month;
                int yearNumber = StringToDate.Year;

                string fullDate = $"{dayNumber - 1}.{monthNumber - 1}.{yearNumber + 1}";
                return fullDate;
            }
    }
}
