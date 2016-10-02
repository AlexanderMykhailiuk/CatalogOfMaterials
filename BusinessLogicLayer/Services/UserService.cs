using BusinessLogicLayer.DataTransferObjects;
using BusinessLogicLayer.Infrastructure;
using DataAccessLayer.Entities;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer;
using static BusinessLogicLayer.Mapping.MappingConfigs;

namespace BusinessLogicLayer.Services
{
    public class UserService : IUserService
    {
        readonly IUnitOfWork Database;
        
        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }
        
        public OperationDetails Create(UserDTO userDto)
        {
            string userName = UserDTOtoUserNameConfig.CreateMapper().Map<string>(userDto);
            User user = Database.UserManager.FindByName(userName);

            if (user == null)
            {
                string email = UserDTOtoEmailConfig.CreateMapper().Map<string>(userDto);
                user = Database.UserManager.FindByEmail(email);
                if (user == null)
                {
                    user = UserDTOtoUserConfig.CreateMapper().Map<User>(userDto);

                    string password = UserDTOtoPaswordConfig.CreateMapper().Map<string>(userDto);
                    Database.UserManager.Create(user, password);

                    Database.UserManager.AddToRole(user.Id, "User");
                    Database.Complete();
                    return new OperationDetails(true, "Registration is successful", "");
                } 
                else return new OperationDetails(false, "User with such E-mail already exist", "Email");
            }
            else
            {
                return new OperationDetails(false, "User with such UserName already exist", "Username");
            }
        }

        public ClaimsIdentity Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;

            string username = UserDTOtoUserNameConfig.CreateMapper().Map<string>(userDto);
            string password = UserDTOtoPaswordConfig.CreateMapper().Map<string>(userDto);
            User user = Database.UserManager.Find(username, password);
            
            if (user != null)
                claim = Database.UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        public OperationDetails ChangePassword(string UserName, string OldPassword, string NewPassword)
        {
            User user = Database.UserManager.FindByName(UserName);

            if (user == null) throw new Exceptions.NotExistUserException(UserName);
            else
            {
                var IsChanged = Database.UserManager.ChangePassword(user.Id, OldPassword, NewPassword).Succeeded;
                if (IsChanged)
                {
                    Database.Complete();
                    return new OperationDetails(true, "Registration is successful", "");
                }
                else return new OperationDetails(false, "User with such UserName already exist", "OldPassword");
            }
        }

        public IEnumerable<RoleDTO> GetAllRoles(string UserName)
        {
            User user_initiator = Database.UserManager.FindByName(UserName);
            if (user_initiator == null) throw new Exceptions.NotExistUserException(UserName);
            else
            {
                var roles = Database.UserManager.GetRoles(user_initiator.Id);

                if (roles.Contains("Admin"))
                {
                    var mapper = UserRoleToUserRoleDTOConfig.CreateMapper();
                    
                    return mapper.Map<IEnumerable<RoleDTO>>(Database.RoleManager.Roles.ToList());
                }
                else throw new Exceptions.NotAllowedOperationForUser(UserName);
            }
        }

        public IEnumerable<UserDTO> GetAllUsers(string UserName)
        {
            User user_initiator = Database.UserManager.FindByName(UserName);
            if (user_initiator == null) throw new Exceptions.NotExistUserException(UserName);
            else
            {
                var roles = Database.UserManager.GetRoles(user_initiator.Id);

                if (roles.Contains("Admin"))
                {
                    var mapper = UserRoleToUserRoleDTOConfig.CreateMapper();

                    return mapper.Map<IEnumerable<UserDTO>>(Database.UserManager.Users.ToList());
                }
                else throw new Exceptions.NotAllowedOperationForUser(UserName);
            }
        }

        public RoleDTO GetRoleOfUser(string UserName, string SearchingUserName)
        {
            User user_initiator = Database.UserManager.FindByName(UserName);
            if (user_initiator == null) throw new Exceptions.NotExistUserException(UserName);
            else
            {
                var roles = Database.UserManager.GetRoles(user_initiator.Id);

                if (roles.Contains("Admin"))
                {
                    User user_in_db = Database.UserManager.FindByName(SearchingUserName);
                    if (user_in_db == null) throw new Exceptions.NotExistUserException(SearchingUserName);
                    else
                    {
                        IList<string> SearchingRoles = Database.UserManager.GetRoles(user_in_db.Id);

                        var mapper = UserRoleToUserRoleDTOConfig.CreateMapper();

                        return mapper.Map<RoleDTO>(SearchingRoles);
                    }
                }
                else throw new Exceptions.NotAllowedOperationForUser(UserName);
            }
        }

        public void GiveRoleToUser(string UserName, string SearchingUserName, RoleDTO role)
        {
            User user_initiator = Database.UserManager.FindByName(UserName);
            if (user_initiator == null) throw new Exceptions.NotExistUserException(UserName);
            else
            {
                var roles = Database.UserManager.GetRoles(user_initiator.Id);

                if (roles.Contains("Admin"))
                {
                    User user_in_db = Database.UserManager.FindByName(SearchingUserName);
                    if (user_in_db == null) throw new Exceptions.NotExistUserException(SearchingUserName);
                    else
                    {
                        if (Database.RoleManager.Roles.Where(x => string.Equals(role.Name, x.Name)).Count() <= 0)
                            throw new Exceptions.NotExistRoleException(role.Name);
                        else
                        {
                            Database.UserManager.AddToRole(user_in_db.Id, role.Name);
                            Database.Complete();
                        }
                    }
                }
                else throw new Exceptions.NotAllowedOperationForUser(UserName);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Database.Dispose();
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
