using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cataloguer.Models;
using Microsoft.AspNetCore.Identity;

namespace Cataloguer.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public DateTime Age { get; set; }

        public double Gender { get; set; } // 0 is female, 1 is male

        public Country Country { get; set; }

        public List<Language> Languages { get; set; }

        public Temperament Temperament { get; set; }
    }
}
