using System;
using System.Collections.Generic;
using System.Text;

namespace DoorToDoorLibrary.Exceptions
{
    class NotMySalespersonException : Exception
    {
        /// <summary>
        /// Constructor needed to create custom exception
        /// </summary>
        /// <param name="message">Custom error message for the exception</param>
        public NotMySalespersonException(string message = "This Salesperson does not work for you") : base(message)
        {

        }
    }
}
