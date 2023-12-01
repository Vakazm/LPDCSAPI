using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace LPDCSAPI.Classes
{
    // Dealers class
    public class Dealer
    {
        // Dealer's registration information class
        public class Reginfo
        {
            // Username, 6-20 character string that may consist of letters, numbers and some special characters
            [MinLength(6)]
            [MaxLength(20)]
            [RegularExpression("""^[a-zA-Z0-9_.-]*$""",
                ErrorMessage = "Username must contail only letters, numbers and characters as '.', '_', '-'")]
            public string? Username { get; set; }

            // Email, 3-100 character string that contains dealer's email address
            [MinLength(3)]
            [MaxLength(100)]
            [EmailAddress]
            [RegularExpression("""^[a-zA-Z0-9_.-@]*$""",
                ErrorMessage = "Email address must contail only letters, numbers and characters as '.', '_', '-', '@'")]
            public string? Email { get; set; }

            // Password, 8-64 character string that consist of lowercase and uppercase letters, numbers and special characters
            [Required]
            [MinLength(8)]
            [MaxLength(64)]
            [RegularExpression("""^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$""",
                ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, one number, and one special character")]
            public string Password { get; set; }
        }

        public Reginfo reginfo { get; set; }  // Dealer registration info, unique for each dealer
        public string Token { get; set; }  // Encrypted access token, unique for each dealer
        public List <Car> Cars { get; set; } = new List<Car>();  // Dealer's collection of cars

        // Function to generate and assign token out of dealer's registration info
        public void GenerateToken()
        {
            Token = Convert.ToBase64String(SHA512.HashData(Encoding.UTF8.GetBytes
                ($"{reginfo.Username}/{reginfo.Email}/{reginfo.Password}")));
        }
    }
}
