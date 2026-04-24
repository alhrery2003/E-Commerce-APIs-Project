using Microsoft.AspNetCore.Identity;

namespace ProductSystem.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }

        public Cart? Cart { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
