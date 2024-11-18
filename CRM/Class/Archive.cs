namespace SRMAgreement.Class
{
    public class Archive_3D
    {
        public int Id { get; set; }
        public int NumberGroup { get; set; }
        public string NameGroup { get; set; }
        public string address { get; set; }
        public string DogovirOrendu { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime StrokDii { get; set; }
        public string Suma { get; set; }
        public DateTime AktDate { get; set; }
        public string? Stan { get; set; }
        public string? Prumitka { get; set; }
    }

    public class Archive_4D
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
        public bool? Done { get; set; }
    }

    public class Archive_5D
    {
        public int Id { get; set; }
        public int NumberGroup { get; set; }
        public string address { get; set; }
        public string OhronnaComp { get; set; }
        public string NumDog { get; set; }
        public DateTime StrokDii { get; set; }
        public string? ResPerson { get; set; }
        public string? Phone { get; set; }
        public string? PathToFile { get; set; }
    }

    public class Archive_6D
    {
        public int Id { get; set; }
        public string NameGroup { get; set; }
        public string NameTov { get; set; }
        public string UnifiedStateRegister { get; set; }
        public string address { get; set; }
        public string director { get; set; }
        public string BanckAccount { get; set; }
        public string? ResPerson { get; set; }
        public string? Phone { get; set; }
    }
}
