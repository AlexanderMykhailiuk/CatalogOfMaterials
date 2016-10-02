using System;
using System.Collections.Generic;
using BusinessLogicLayer.DataTransferObjects;
using BusinessLogicLayer.Infrastructure;
using System.Security.Claims;

namespace BusinessLogicLayer.Interfaces
{
    public interface IUserService : IDisposable
    {
        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="userDto">Data about new user (e-mail,passsword,username)</param>
        /// <returns>Details of creation</returns>
        OperationDetails Create(UserDTO userDto);
        
        /// <summary>
        /// Authenticate new user
        /// </summary>
        /// <param name="userDto">Data about user (username and password)</param>
        /// <returns>Claims for identity</returns>
        ClaimsIdentity Authenticate(UserDTO userDto);

        /// <summary>
        /// Change password of user
        /// </summary>
        /// <param name="Username">Username of user</param>
        /// <param name="OldPassword">Old password</param>
        /// <param name="NewPassword">New password</param>
        /// <exception cref="Exceptions.NotExistUserException">Thrown if user not exists</exception>
        OperationDetails ChangePassword(string UserName, string OldPassword, string NewPassword);

        /// <summary>
        /// Return all roles in system
        /// </summary>
        /// <param name="Username">Username of user who initiate operation</param>
        /// <exception cref="Exceptions.NotExistUserException">Thrown when user not exist</exception>
        /// <exception cref="Exceptions.NotAllowedOperationForUser">Thrown when user can't make such operation</exception>
        IEnumerable<RoleDTO> GetAllRoles(string UserName);

        /// <summary>
        /// Return all user in system
        /// </summary>
        /// <param name="Username">Username of user who initiate operation</param>
        /// <exception cref="Exceptions.NotExistUserException">Thrown when user not exist</exception>
        /// <exception cref="Exceptions.NotAllowedOperationForUser">Thrown when user can't make such operation</exception>
        IEnumerable<UserDTO> GetAllUsers(string UserName);

        /// <summary>
        /// Return role of searching user
        /// </summary>
        /// <param name="Username">Username of user who initiate operation</param>
        /// <param name="SearchingUserName">UserName of searching user</param>
        /// <exception cref="Exceptions.NotExistUserException">Thrown when user not exist</exception>
        /// <exception cref="Exceptions.NotAllowedOperationForUser">Thrown when user can't make such operation</exception>
        RoleDTO GetRoleOfUser(string UserName, string SearchingUserName);

        /// <summary>
        /// Give role to searching user
        /// </summary>
        /// <param name="UserName">Username of user who initiate operation</param>
        /// <param name="SearchingUserName">Username of user whome give role</param>
        /// <param name="role">New role</param>
        /// <exception cref="Exceptions.NotExistUserException">Thrown when user not exist</exception>
        /// <exception cref="Exceptions.NotAllowedOperationForUser">Thrown when user can't make such operation</exception>
        /// <exception cref="Exceptions.NotExistRoleException">Thrown when role not exist</exception>
        void GiveRoleToUser(string UserName, string SearchingUserName, RoleDTO role);
    }
}
