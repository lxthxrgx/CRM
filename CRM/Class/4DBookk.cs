namespace SRMAgreement.Class
{
    public class _4DBook
    {
        public int Id { get; set; }
        public int NumberGroup { get; set; }
        public string NameGroup { get; set; }
        public string address { get; set; }
        public string DogovirSuborendu { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime AktDate { get; set; }
        public DateTime EndAktDate { get; set; }
        public string Suma { get; set; }
        public string? Suma2 { get; set; }
        public DateTime? Updatet_Record { get; set; }
        public int IdFromD4 { get; set; }

        public DateTime? payments_term { get; set; }
    }
}