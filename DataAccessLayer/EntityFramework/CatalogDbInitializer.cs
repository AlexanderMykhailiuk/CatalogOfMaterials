using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using DataAccessLayer.Entities;
using Microsoft.AspNet.Identity;

namespace DataAccessLayer.EntityFramework
{

    class CatalogDbInitializer : DropCreateDatabaseIfModelChanges<CatalogContext>
    {
        protected override void Seed(CatalogContext db)
        {
            // Add role for simple users
            if (!db.Roles.Any(r => r.Name == "User"))
            {
                var store = new RoleStore<Role>(db);
                var manager = new RoleManager<Role>(store);
                var role = new Role { Name = "User" };

                manager.Create(role);
            }

            // Add role for moderator
            if (!db.Roles.Any(r => r.Name == "Moderator"))
            {
                var store = new RoleStore<Role>(db);
                var manager = new RoleManager<Role>(store);
                var role = new Role { Name = "Moderator" };

                manager.Create(role);
            }

            // Add role for admins
            if (!db.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<Role>(db);
                var manager = new RoleManager<Role>(store);
                var role = new Role { Name = "Admin" };

                manager.Create(role);
            }

            // Create admin
            if (!db.Users.Any(u => u.UserName == "admin"))
            {
                var store = new UserStore<User>(db);
                var manager = new UserManager<User>(store);
                var user = new User { UserName = "Admin", Email = "admin@admins.com" };

                manager.Create(user, "111111");
                manager.AddToRole(user.Id, "Admin");
            }

            // Create genres
            if (db.Genres.Count() < 1)
            {
                db.Genres.Add(new Genre { Name = "Pop" });
                db.Genres.Add(new Genre { Name = "Rock" });
                db.Genres.Add(new Genre { Name = "Hip-hop" });
            }
        }
    }
}
