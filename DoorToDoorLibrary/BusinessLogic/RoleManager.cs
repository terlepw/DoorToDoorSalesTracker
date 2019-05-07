using DoorToDoorLibrary.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorToDoorLibrary.Logic
{
    /// <summary>
    /// Holds a user and manages their permissions
    /// </summary>
    public class RoleManager
    {
        /// <summary>
        /// The available user roles
        /// </summary>
        public enum eRole
        {
            Unknown = 0,
            Administrator = 1,
            Manager = 2,
            Salesperson = 3
        }

        /// <summary>
        /// The user to manage permissions for
        /// </summary>
        public UserItem User { get; }

        /// <summary>
        /// The name of the user's role
        /// </summary>
        public eRole RoleName { get; }

        /// <summary>
        /// Constructor for the role manager. Create this everytime a user changes.
        /// </summary>
        /// <param name="user">The user to get the permissions for</param>
        public RoleManager(UserItem user)
        {
            User = user;

            if (user != null)
            {
                RoleName = (eRole)user.RoleId;
            }
            else
            {
                RoleName = eRole.Unknown;
            }
        }

        /// <summary>
        /// Specifies if the user has Administrator permissions
        /// </summary>
        public bool IsAdministrator
        {
            get
            {
                return RoleName == eRole.Administrator;
            }
        }

        /// <summary>
        /// Specifies if the user has Manager permissions
        /// </summary>
        public bool IsManager
        {
            get
            {
                return RoleName == eRole.Manager;
            }
        }

        /// <summary>
        /// Specifies if the user has Salesperson permissions
        /// </summary>
        public bool IsSalesperson
        {
            get
            {
                return RoleName == eRole.Salesperson;
            }
        }

        /// <summary>
        /// Specifies if the user has unknown permissions
        /// </summary>
        public bool IsUnknown
        {
            get
            {
                return RoleName == eRole.Unknown;
            }
        }
    }
}
