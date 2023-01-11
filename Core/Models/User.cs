namespace Core.Models
{
  public class User : BaseModel
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? LastLogout { get; set; }
    }
}
