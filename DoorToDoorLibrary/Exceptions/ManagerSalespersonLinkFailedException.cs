using System;
using System.Collections.Generic;
using System.Text;

namespace DoorToDoorLibrary.Exceptions
{
    class ManagerSalespersonLinkFailedException : Exception
    {
        /// <summary>
        /// Constructor needed to create custom exception
        /// </summary>
        /// <param name="message">Custom error message for the exception</param>
        public ManagerSalespersonLinkFailedException(string message = "Failed to link the Manager with the Salesperson") : base(message)
        {

        }
    }
}
