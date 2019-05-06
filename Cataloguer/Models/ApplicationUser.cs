using Microsoft.AspNetCore.Identity;

namespace Cataloguer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int BirthYear { get; set; }

        public double Gender { get; set; } // 0 is female, 1 is male

        public int CountryId { get; set; }
        public Country Country { get; set; }

        public int SecondLanguageId { get; set; }
        public Language SecondLanguage { get; set; }

        public int TemperamentId { get; set; }
        public Temperament Temperament { get; set; }
    }
}
