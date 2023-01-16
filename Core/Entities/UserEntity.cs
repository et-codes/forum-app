using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class UserEntity : IdentityUser
    {
        public string DisplayName { get; set; }
    }
}
