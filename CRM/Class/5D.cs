using Microsoft.Extensions.Hosting;

namespace SRMAgreement.Class
{
    public class _5D
    {
        public int Id { get; set; }
        public int NumberGroup { get; set; }
        public string NameGroup { get; set; }
        public string address { get; set; }
        public string OhronnaComp { get; set; }
        public string NumDog { get; set; }
        public string? NumDog2 { get; set; }
        public DateTime StrokDii { get; set; }
        public DateTime? StrokDii2 { get; set; }
        public string? ResPerson { get; set; }
        public string? Phone { get; set; }

        public ICollection<PathToFilesGuard> PathToFilesGuard { get; } = new List<PathToFilesGuard>();
    }

    public class PathToFilesGuard
    {
        public int Id { get; set; }

        public int _5dId { get; set; }
        public _5D D5class { get; set; }
        public string? PathTOServerFiles { get; set; }
    }

}
