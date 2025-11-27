namespace Tesko.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } // "Requester", "Approver", "Admin"
        public string PasswordHash { get; set; }
    }
}
