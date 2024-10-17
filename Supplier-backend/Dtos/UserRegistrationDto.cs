namespace Supplier_backend.Dtos
{
    public class UserRegistrationDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // e.g., "admin", "staff"
    }
}
