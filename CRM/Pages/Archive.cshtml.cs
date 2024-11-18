using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SRMAgreement.Class;
using SRMAgreement.Data_Base;

namespace SRMAgreement.Pages
{
    [Authorize]
    public class ArchiveModel : PageModel
    {
        private readonly DataBaseArchive _archive;
        public ArchiveModel(DataBaseArchive archive)
        {
            _archive = archive;
            AD4 = new List<Archive_4D>();
            AD3 = new List<Archive_3D>();
            AD5 = new List<Archive_5D>();
            AD6 = new List<Archive_6D>();
        }
        [BindProperty]
        public List<Archive_4D> AD4 { get; set; }
        public List<Archive_3D> AD3 { get; set; }
        public List<Archive_5D> AD5 { get; set; }
        public List<Archive_6D> AD6 { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; }
        public List<Archive_4D> SearchResults { get; set; }


        public async Task OnGetAsync()
        {
            AD4 = await _archive.Archive_4D
             .OrderBy(d => d.NumberGroup)
             .ToListAsync();

            AD3 = await _archive.Archive_3D
             .OrderBy(d => d.NumberGroup)
             .ToListAsync();

            AD5 = await _archive.Archive_5D
             .OrderBy(d => d.NumberGroup)
             .ToListAsync();

            AD6 = await _archive.Archive_6D.ToListAsync();
        }
        //public async Task<IActionResult> OnPostAsync(string action)
        //{
        //if (!ModelState.IsValid)
        //{
        //    return Page();
        //}
        //if(action == "Search")
        //{
        //    if (string.IsNullOrEmpty(SearchQuery))
        //    {
        //        return RedirectToPage();
        //    }

        //    var SearchResults3D = _archive.Archive_3D
        //       .Where(e => e.NumberGroup2D.ToString().Contains(SearchQuery) ||
        //               e.NameGroup2D.Contains(SearchQuery) ||
        //               e.address.Contains(SearchQuery) ||
        //               e.PIB.Contains(SearchQuery) ||
        //               e.DogovirOrendu.Contains(SearchQuery) ||
        //               e.DateTime.ToString().Contains(SearchQuery) ||
        //               e.StrokDii.ToString().Contains(SearchQuery) ||
        //               e.Suma.ToString().Contains(SearchQuery) ||
        //               e.AktDate.ToString().Contains(SearchQuery) ||
        //               e.Stan.Contains(SearchQuery) ||
        //               e.Prumitka.Contains(SearchQuery)
        //                   )
        //       .Select(e => new SearchResult
        //       {
        //           EntityType = "Archive_3D",
        //           NumberGroup2D = e.NumberGroup2D,
        //           NameGroup2D = e.NameGroup2D,
        //           address = e.address,
        //           PIB = e.PIB,
        //           DogovirOrendu = e.DogovirOrendu,
        //           DateTime = e.DateTime,
        //           StrokDii = e.StrokDii,
        //           Suma = e.Suma,
        //           AktDate = e.AktDate,
        //           Stan = e.Stan,
        //           Prumitka = e.Prumitka
        //       });

        //    var SearchResults4D = _archive.Archive_4D
        //       .Where(e => e.NumberGroup2D.ToString().Contains(SearchQuery) ||
        //               e.NameGroup2D.Contains(SearchQuery) ||
        //               e.address.Contains(SearchQuery) ||
        //               e.PIB.Contains(SearchQuery) ||
        //               e.DogovirSuborendu.Contains(SearchQuery) ||
        //               e.DateTime.ToString().Contains(SearchQuery) ||
        //               e.StrokDii.ToString().Contains(SearchQuery) ||
        //               e.Suma.ToString().Contains(SearchQuery) ||
        //               e.AktDate.ToString().Contains(SearchQuery) ||
        //               e.Stan.Contains(SearchQuery) ||
        //               e.Prumitka.Contains(SearchQuery)
        //                   ).Select(e => new SearchResult
        //                   {
        //                       EntityType = "Archive_4D",
        //                       NumberGroup2D = e.NumberGroup2D,
        //                       NameGroup2D = e.NameGroup2D,
        //                       address = e.address,
        //                       PIB = e.PIB,
        //                       DogovirOrendu = e.DogovirSuborendu,
        //                       DateTime = e.DateTime,
        //                       StrokDii = e.StrokDii,
        //                       Suma = e.Suma,
        //                       AktDate = e.AktDate,
        //                       Stan = e.Stan,
        //                       Prumitka = e.Prumitka
        //                   });

        //    var SearchResults5D =  _archive.Archive_5D
        //       .Where(e => e.NumberGroup2D.ToString().Contains(SearchQuery) ||
        //               e.address.Contains(SearchQuery) ||
        //               e.PIB.Contains(SearchQuery) ||
        //               e.OhronnaComp.Contains(SearchQuery) ||
        //               e.NumDog.Contains(SearchQuery) ||
        //               e.StrokDii.ToString().Contains(SearchQuery) ||
        //               e.ResPerson.ToString().Contains(SearchQuery) ||
        //               e.Phone.ToString().Contains(SearchQuery) 
        //                   ).Select(e => new SearchResult
        //                    {
        //                       EntityType = "Archive_5D",
        //                       NumberGroup2D = e.NumberGroup2D,
        //                       address = e.address,
        //                       PIB = e.PIB,
        //                       OhronnaComp = e.OhronnaComp,
        //                       NumDog = e.NumDog,
        //                       StrokDii = e.StrokDii,
        //                       ResPerson = e.ResPerson,
        //                       Phone = e.Phone
        //                   });

        //    var SearchResults6D = _archive.Archive_6D
        //      .Where(e => e.NameGroup.ToString().Contains(SearchQuery) ||
        //              e.NameTov.Contains(SearchQuery) ||
        //              e.UnifiedStateRegister.Contains(SearchQuery) ||
        //              e.address.Contains(SearchQuery) ||
        //              e.director.Contains(SearchQuery) ||
        //              e.BanckAccount.ToString().Contains(SearchQuery) ||
        //              e.ResPerson.ToString().Contains(SearchQuery) ||
        //              e.Phone.ToString().Contains(SearchQuery)
        //                  ).Select(e => new SearchResult
        //                  {
        //                      EntityType = "Archive_6D",
        //                      NameTov = e.NameTov,
        //                      UnifiedStateRegister = e.UnifiedStateRegister,
        //                      address = e.address,
        //                      director = e.director,
        //                      BanckAccount = e.BanckAccount,
        //                      ResPerson = e.ResPerson,
        //                      Phone = e.Phone
        //                  });
        //    SearchResults.AddRange(SearchResults3D);
        //    SearchResults.AddRange(SearchResults4D);
        //    SearchResults.AddRange(SearchResults5D);
        //    SearchResults.AddRange(SearchResults6D);
        //    return Page();

        //}
        //return Page();
        //    }
    }
}
