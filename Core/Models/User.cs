using Microsoft.AspNetCore.Identity;

namespace Core.Models
{
  public class User : IdentityUser
    {
        public string DisplayName { get; set; }
    }
}
