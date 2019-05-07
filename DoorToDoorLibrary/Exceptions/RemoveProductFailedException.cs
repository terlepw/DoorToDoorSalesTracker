using System;
using System.Collections.Generic;
using System.Text;

namespace DoorToDoorLibrary.Exceptions
{
    public class RemoveProductFailedException : Exception
    {
        /// <summary>
        /// Constructor needed to create custom exception
        /// </summary>
        /// <param name="message">Custom error message for the exception</param>
        public RemoveProductFailedException(string message = "Failed to remove the Product") : base(message)
        {

        }
    }
}
