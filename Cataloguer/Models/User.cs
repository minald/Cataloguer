using Microsoft.AspNetCore.Identity;

namespace Cataloguer.Models
{
    public class User : IdentityUser
    {
        public string Country { get; set; }
    }
}
