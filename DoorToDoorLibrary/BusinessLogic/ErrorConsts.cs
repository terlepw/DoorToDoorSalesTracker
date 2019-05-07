using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorLibrary.Logic
{
    public class ErrorConsts
    {
        /// <summary>
        /// Error message for blank input fields
        /// </summary>
        public const string BlankError = "Field must not be blank";

        /// <summary>
        /// Error message for password mismatches
        /// </summary>
        public const string NoPasswordMatch = "Password and Confirm Password must match";

        /// <summary>
        /// Integer for ten character input field limit
        /// </summary>
        public const int MaxCharTen = 10;

        /// <summary>
        /// Error message for exceeding ten characters
        /// </summary>
        public const string MaxTenError = "Field must not exceed 10 characters";

        /// <summary>
        /// Integer for fifty character input field limit
        /// </summary>
        public const int MaxCharFifty = 50;

        /// <summary>
        /// Error message for exceeding fifty characters
        /// </summary>
        public const string MaxFiftyError = "Field must not exceed 50 characters";

        /// <summary>
        /// Integer for 100 character input field limit
        /// </summary>
        public const int MaxCharHundred = 100;

        /// <summary>
        /// Error message for exceeding 100 characters
        /// </summary>
        public const string MaxHundredError = "Field must not exceed 100 characters";

        /// <summary>
        /// Integer for 255 character input field limit
        /// </summary>
        public const int MaxChar255 = 255;

        /// <summary>
        /// Error message for exceeding 255 characters
        /// </summary>
        public const string Max255Error = "Field must not exceed 255 characters";
    }
}
