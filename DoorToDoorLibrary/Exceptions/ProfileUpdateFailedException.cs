using System;
using System.Collections.Generic;
using System.Text;

namespace DoorToDoorLibrary.Exceptions
{
    public class ProfileUpdateFailedException : Exception
    {
        /// <summary>
        /// Constructor needed to create custom exception
        /// </summary>
        /// <param name="message">Custom error message for the exception</param>
        public ProfileUpdateFailedException(string message = "Failed to update Profile") : base(message)
        {

        }
    }
}
