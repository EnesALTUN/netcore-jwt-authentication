using Microsoft.AspNetCore.Identity;

namespace UdemyAuthServer.Core.Entities
{
    public class UserApp : IdentityUser
    {
        public string? City { get; set; }
    }
}