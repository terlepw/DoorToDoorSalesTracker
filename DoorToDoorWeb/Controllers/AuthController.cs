using System;
using DoorToDoorLibrary.DAL;
using DoorToDoorLibrary.DatabaseObjects;
using DoorToDoorLibrary.Exceptions;
using DoorToDoorLibrary.Logic;
using DoorToDoorLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace DoorToDoorWeb.Controllers
{
    public class AuthController : SessionController
    {
        /// <summary>
        /// Manages the user authentication and authorization
        /// </summary>
        private RoleManager _roleMgr = null;
        protected IDoorToDoorDAL _db = null;
        private const string RoleMgrKey = "RoleManager";

        public AuthController(IDoorToDoorDAL db, IHttpContextAccessor httpContext) : base(httpContext)
        {
            _db = db;

            // Get the role manager from the session
            _roleMgr = GetSessionData<RoleManager>(RoleMgrKey);

            // If it does not exist on the session then add it
            if(_roleMgr == null)
            {
                // Since the role manager is being created then a user still needs to be authenticated
                _roleMgr = new RoleManager(null);

                SetSessionData(RoleMgrKey, _roleMgr);
            }
        }

        /// <summary>
        /// The current logged in user of the Door to Door sales tracker
        /// </summary>
        public UserItem CurrentUser
        {
            get
            {
                return _roleMgr.User;
            }
        }

        /// <summary>
        /// RoleManager used to validate user permissions and have access to the current user information
        /// </summary>
        public RoleManager Role
        {
            get
            {
                return _roleMgr;
            }
        }

        /// <summary>
        /// Returns true if the Door to Door sales tracker has an authenticated user
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return _roleMgr.User != null;
            }
        }

        /// <summary>
        /// Adds a new user to the Door to Door sales system
        /// </summary>
        /// <param name="userModel">Model that contains all the user information</param>
        public int RegisterUser(User userModel)
        {
            UserItem userItem = null;
            try
            {
                userItem = _db.GetUserItem(userModel.EmailAddress);
            }
            catch (Exception)
            {
            }

            if (userItem != null)
            {
                throw new UserExistsException("The email is already taken.");
            }

            if (userModel.Password != userModel.ConfirmPassword)
            {
                throw new PasswordMatchException("The password and confirm password do not match.");
            }

            PasswordManager passHelper = new PasswordManager(userModel.Password);
            UserItem newUser = new UserItem()
            {
                FirstName = userModel.FirstName,
                LastName = userModel.LastName,
                EmailAddress = userModel.EmailAddress,
                Salt = passHelper.Salt,
                Hash = passHelper.Hash,
                RoleId = userModel.RoleId
            };
            
            return _db.RegisterNewUser(newUser);
        }

        /// <summary>
        /// Logs a user into the Door to Door sales tracker system and throws exceptions on any failures
        /// </summary>
        /// <param name="username">The username of the user to authenicate</param>
        /// <param name="password">The password of the user to authenicate</param>
        public void LoginUser(string emailAddress, string password)
        {
            UserItem user = null;

            try
            {
                user = _db.GetUserItem(emailAddress);
            }
            catch (Exception)
            {
                throw new Exception("Either the username or the password is invalid.");
            }

            PasswordManager passHelper = new PasswordManager(password, user.Salt);
            if (!passHelper.Verify(user.Hash))
            {
                throw new Exception("Either the username or the password is invalid.");
            }

            _roleMgr = new RoleManager(user);

            // Put the authenticated user in the session
            SetSessionData(RoleMgrKey, _roleMgr);
        }

        /// <summary>
        /// Logs the current user out of the Door to Door Sales Tracker system
        /// </summary>
        public void LogoutUser()
        {
            _roleMgr = new RoleManager(null);
            
            SetSessionData(RoleMgrKey, _roleMgr);
        }

        public ActionResult GetAuthenticatedView(string viewName, object model = null)
        {
            ActionResult result = null;
            if (IsAuthenticated)
            {
                result = View(viewName, model);
            }
            else
            {
                result = RedirectToAction("Login", "Home");
            }
            return result;
        }

        public JsonResult GetAuthenticatedJson(JsonResult json, bool hasPermission)
        {
            JsonResult result = null;
            if (!hasPermission && IsAuthenticated)
            {
                result = Json(new { error = "User is not permitted to access this data." });
            }
            else if (IsAuthenticated)
            {
                result = json;
            }
            else
            {
                result = Json(new { error = "User is not authenticated." });
            }
            return result;
        }

        public string GenerateNewPassword()
        {
            Random rnd = new Random();

            return $"Password{rnd.Next(101)}";
        }
    }
}
