using DataAccessLayer.EntityFramework;
using DataAccessLayer.Intetfaces;
using DataAccessLayer.Repositories;
using DataAccessLayer.Entities;
using DataAccessLayer.IdentityManagers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DataAccessLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CatalogContext _context;

        public UnitOfWork(string connectionString)
        {
            _context = new CatalogContext(connectionString);
            Contents = new Repository<Content>(_context);
            Files = new Repository<File>(_context);
            Genres = new Repository<Genre>(_context);
            RoleManager = new AppRoleManager(new RoleStore<Role>(_context));
            UserManager = new AppUserManager(new UserStore<User>(_context));
        }

        public IRepository<Content> Contents { get; private set; }

        public IRepository<File> Files { get; private set; }

        public IRepository<Genre> Genres { get; private set; }

        public RoleManager<Role> RoleManager { get; private set; }

        public UserManager<User> UserManager { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
