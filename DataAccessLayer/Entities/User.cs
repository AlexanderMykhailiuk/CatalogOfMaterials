using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DataAccessLayer.Entities
{
    public class User : IdentityUser
    {
        public virtual ICollection<Content> Contents { get; set; }
        public User()
        {
            Contents = new List<Content>();
        }
    }
}
