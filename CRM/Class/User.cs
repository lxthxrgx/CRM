namespace SRMAgreement.Class
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password {get; set; }
        public string Role { get; set; }
    }

    public class UserStatus
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastActivity { get; set; }
    }
}
