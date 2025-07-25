namespace LibrarySystem.Models
{
    public class LoginRequest
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Pass { get; set; }
        public string Role { get; set; } 
    }
}
