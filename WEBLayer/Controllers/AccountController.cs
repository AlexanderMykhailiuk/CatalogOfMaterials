using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using BusinessLogicLayer.DataTransferObjects;
using BusinessLogicLayer.Infrastructure;
using WEBLayer.Models;
using static WEBLayer.Mapping.MappingConfigs;

namespace WEBLayer.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// Get UserService for manipulating with users
        /// </summary>
        private IUserService userService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }

        /// <summary>
        /// Get authentification manager
        /// </summary>
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        /// <summary>
        /// Return view with registry form
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        

        /// <summary>
        /// Registry new user if all correct
        /// </summary>
        /// <param name="model">Data about new user</param>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {            
            if (ModelState.IsValid)
            {
                UserDTO userDto = RegisterModeltoUserDTOConfig.CreateMapper().Map<UserDTO>(model);
                OperationDetails operationDetails = userService.Create(userDto);
                if (operationDetails.Succedeed)
                {
                    ClaimsIdentity claim = userService.Authenticate(userDto);
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }

        /// <summary>
        /// Return view for logging in
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        

        /// <summary>
        /// Logging in user
        /// </summary>
        /// <param name="model">Contain username and password of user</param>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDto = LoginModeltoUserDTOConfig.CreateMapper().Map<UserDTO>(model);
                ClaimsIdentity claim = userService.Authenticate(userDto);
                if (claim == null)
                {
                    ModelState.AddModelError("", "Not correct username or password");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        /// <summary>
        /// Logging out user
        /// </summary>
        [HttpGet]
        [Authorize]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {   
                OperationDetails operationDetails = userService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                if (operationDetails.Succedeed)
                {
                    UserDTO userDto = LoginModeltoUserDTOConfig.CreateMapper().Map<UserDTO>(new LoginModel()
                    {
                        Username = User.Identity.Name,
                        Password = model.NewPassword
                    });
                    ClaimsIdentity claim = userService.Authenticate(userDto);
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult ManageUsers()
        {
            try
            {
                var mapper = UserManageConfig.CreateMapper();

                Dictionary<string, RoleModel> dictionaryRole = new Dictionary<string, RoleModel>();

                var allUsers = mapper.Map<IEnumerable<AboutUserModel>>(userService.GetAllUsers(User.Identity.Name));

                foreach (var user in allUsers)
                {
                    RoleModel userRole = mapper.Map<RoleModel>(userService.GetRoleOfUser(
                        User.Identity.Name,
                        user.UserName));
                    dictionaryRole.Add(user.UserName, userRole);
                }

                ViewBag.dictionaryRole = dictionaryRole;
                ViewBag.Roles = mapper.Map<IEnumerable<RoleModel>>(userService.GetAllRoles(User.Identity.Name));

                return View(allUsers);
            }
            catch (BusinessLogicLayer.Exceptions.NotAllowedOperationForUser)
            {
                return HttpNotFound();
            }
            catch (BusinessLogicLayer.Exceptions.NotExistUserException)
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddToRole(string UserName, string RoleName)
        {
            if (UserName == null) return HttpNotFound();
            try
            {
                var mapper = UserManageConfig.CreateMapper();

                RoleDTO role = mapper.Map<RoleDTO>(
                    new RoleModel() { Name = RoleName });

                userService.GiveRoleToUser(User.Identity.Name, UserName, role);

                return RedirectToAction("ManageUsers", "Account");
            }
            catch (BusinessLogicLayer.Exceptions.NotAllowedOperationForUser)
            {
                return HttpNotFound();
            }
            catch (BusinessLogicLayer.Exceptions.NotExistUserException)
            {
                return HttpNotFound();
            }
            catch (BusinessLogicLayer.Exceptions.NotExistGenreException)
            {
                return HttpNotFound();
            }
        }
    }
}