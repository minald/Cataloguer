using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Cataloguer.Models
{
    public class User : IdentityUser
    {
        public DateTime Age { get; set; }

        public double Gender { get; set; } // 0 is female, 1 is male

        public Country Country { get; set; }

        public List<Language> Languages { get; set; }

        public Temperament Temperament { get; set; }
    }
}
