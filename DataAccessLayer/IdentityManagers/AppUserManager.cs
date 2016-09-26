using DataAccessLayer.Entities;
using Microsoft.AspNet.Identity;

namespace DataAccessLayer.IdentityManagers
{
    public class AppUserManager : UserManager<User>
    {
        public AppUserManager(IUserStore<User> store)
                : base(store)
        {
        }
    }
}
