using System.ComponentModel.DataAnnotations;

namespace Cataloguer.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
