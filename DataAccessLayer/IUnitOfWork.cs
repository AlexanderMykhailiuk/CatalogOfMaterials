using System;
using DataAccessLayer.Intetfaces;
using Microsoft.AspNet.Identity;
using DataAccessLayer.Entities;

namespace DataAccessLayer
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// UserManager for manipulating with users
        /// </summary>
        UserManager<User> UserManager { get; }

        /// <summary>
        /// RoleMAnager for manipulating with roles
        /// </summary>
        RoleManager<Role> RoleManager { get; }

        /// <summary>
        /// Give access to all genres in DB
        /// </summary>
        IRepository<Genre> Genres { get; }

        /// <summary>
        /// Give access to all contents in DB
        /// </summary>
        IRepository<Content> Contents { get; }
        
        /// <summary>
        /// Give access to all files in DB
        /// </summary>
        IRepository<File> Files { get; }
        
        /// <summary>
        /// Save changes in DB
        /// </summary>
        int Complete();
    }
}
