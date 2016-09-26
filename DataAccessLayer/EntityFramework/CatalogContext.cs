using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using DataAccessLayer.Entities;

namespace DataAccessLayer.EntityFramework
{
    class CatalogContext : IdentityDbContext<User>
    {
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<File> Files { get; set; }

        static CatalogContext()
        {
            Database.SetInitializer<CatalogContext>(new CatalogDbInitializer());

            //The Entity Framework provider type 'System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer'
            //for the 'System.Data.SqlClient' ADO.NET provider could not be loaded. 
            //Make sure the provider assembly is available to the running application. 
            //See http://go.microsoft.com/fwlink/?LinkId=260882 for more information.

            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
        public CatalogContext(string connectionString)
            : base(connectionString)
        {
        }
    }
}
