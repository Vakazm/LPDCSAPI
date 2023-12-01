using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LPDCSAPI
{
    // Cars class
    public class Car
    {
        // Car ID, unique for every one in dealer's collection
        [JsonIgnore]
        public int? ID { get; set; }

        // Car make, 1-100 character string that may consist of letters, numbers and some special characters
        [Required]
        [MinLength (1)]
        [MaxLength (100)]
        [RegularExpression ("""^[a-zA-Z0-9_.-]*$""",
                ErrorMessage = "Make must contail only letters, numbers and characters as '.', '_', '-'")]
        public string Make { get; set; }

        // Car model, 1-100 character string that may consist of letters, numbers and some special characters
        [Required]
        [MinLength (1)]
        [MaxLength (100)]
        [RegularExpression ("""^[a-zA-Z0-9_.-]*$""",
                ErrorMessage = "Model must contail only letters, numbers and characters as '.', '_', '-'")]
        public string Model { get; set; }

        // Car year, an integer variable in range 1880-2030
        [Required]
        [Range (1880, 2030)]
        public int Year { get; set; }
    }

}
