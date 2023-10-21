using Microsoft.AspNetCore.Identity;

namespace Project.Models;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public ICollection<Order> Orders { get; } = new List<Order>();
}