using System;
using System.Collections.Generic;
using System.Text;

namespace DoorToDoorLibrary.Exceptions
{
    class NotMyHouseException : Exception
    {
        /// <summary>
        /// Constructor needed to create custom exception
        /// </summary>
        /// <param name="message">Custom error message for the exception</param>
        public NotMyHouseException(string message = "The selected House does not belong to you") : base(message)
        {

        }
    }
}
