namespace SRMAgreement.Class
{
    public class _4D
    {
        public int Id { get; set; }
        public int NumberGroup { get; set; }
        public string NameGroup { get; set; }
        public string address { get; set; }
        public string DogovirSuborendu { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime EndAktDate { get; set; }
        public string Suma { get; set; }
        public string? Suma2 { get; set; }
        public DateTime AktDate { get; set; }

        public ICollection<PdfFilePath_Sublease> PathToPdfFiles_Sublease { get; } = new List<PdfFilePath_Sublease>();

        public bool? Done { get; set; } = false;

    }

    public class PdfFilePath_Sublease
    {
        public int Id { get; set; }
        public int _4DId { get; set; }
        public _4D _4D { get; set; }
        public string? PathToPdfFile_Sublease { get; set; }

    }
}