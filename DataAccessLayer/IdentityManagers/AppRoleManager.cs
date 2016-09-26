using DataAccessLayer.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DataAccessLayer.IdentityManagers
{
    public class AppRoleManager : RoleManager<Role>
    {
        public AppRoleManager(RoleStore<Role> store)
                    : base(store)
        { }
    }
}
