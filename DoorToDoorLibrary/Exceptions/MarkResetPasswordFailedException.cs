using System;
using System.Collections.Generic;
using System.Text;

namespace DoorToDoorLibrary.Exceptions
{
    /// <summary>
    /// Specifies that the Password Reset flag failed to be reset
    /// </summary>
    class MarkResetPasswordFailedException : Exception
    {
        /// <summary>
        /// Constructor needed to create custom exception
        /// </summary>
        /// <param name="message">Custom error message for the exception</param>
        public MarkResetPasswordFailedException(string message = "Failed to set Reset Password flag") : base(message)
        {

        }
    }
}
