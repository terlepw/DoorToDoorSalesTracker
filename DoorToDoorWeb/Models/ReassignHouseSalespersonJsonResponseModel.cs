using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoorToDoorWeb.Models
{
    public class ReassignHouseSalespersonJsonResponseModel
    {
        private const string _successMessage = "Salesperson Successfully Updated!";
        private const string _failureMessage = "Salesperson Failed To Update.";

        public bool UpdateSuccessful { get; }
        public string Message { get; }

        /// <summary>
        /// Creates a ReassignHouseSalespersonJsonResponseModel setting default Messages based on whether or not the Salesperson Update was successful
        /// </summary>
        /// <param name="wasSuccessful">Whether or not the Reassign House Salesperson was successful</param>
        public ReassignHouseSalespersonJsonResponseModel(bool wasSuccessful)
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
        /// Creates a ReassignHouseSalespersonJsonResponseModel as failed with a custom Failed message
        /// </summary>
        /// <param name="failureMessage">Custom Failure Message</param>
        public ReassignHouseSalespersonJsonResponseModel(string failureMessage)
        {
            this.UpdateSuccessful = false;
            this.Message = failureMessage;
        }
    }
}
