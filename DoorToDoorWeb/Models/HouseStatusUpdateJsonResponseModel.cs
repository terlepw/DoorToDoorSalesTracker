using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class HouseStatusUpdateJsonResponseModel
    {
        private const string _successMessage = "Status Successfully Updated!";
        private const string _failureMessage = "Status Failed To Update.";

        public bool UpdateSuccessful { get; }
        public string Message { get; }

        /// <summary>
        /// Creates a HouseStatusUpdateJsonResponseModel setting default Messages based on whether or not the Status Update was successful
        /// </summary>
        /// <param name="wasSuccessful">Whether or not the House Status Update was successful</param>
        public HouseStatusUpdateJsonResponseModel(bool wasSuccessful)
        {
            this.UpdateSuccessful = wasSuccessful;

            if (wasSuccessful)
            {
                this.Message = _successMessage;
            }
            else
            {
                this.Message = _failureMessage;
            }
        }

        /// <summary>
        /// Creates a HouseStatusUpdateJsonResponseModel as failed with a custom Failed message
        /// </summary>
        /// <param name="failureMessage">Custom Failure Message</param>
        public HouseStatusUpdateJsonResponseModel(string failureMessage)
        {
            this.UpdateSuccessful = false;
            this.Message = failureMessage;
        }
    }
}
