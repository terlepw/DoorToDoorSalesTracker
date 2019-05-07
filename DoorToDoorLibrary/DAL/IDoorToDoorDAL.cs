using DoorToDoorLibrary.DatabaseObjects;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoorToDoorLibrary.DAL
{
    public interface IDoorToDoorDAL
    {
        #region User Methods

        /// <summary>
        /// Finds a single user in the database using the user's Email Address
        /// </summary>
        /// <param name="emailAddress">The desired user's Email Address</param>
        /// <returns>UserItem containing the user's information</returns>
        UserItem GetUserItem(string emailAddress);

        /// <summary>
        /// Creates a new User in the database
        /// </summary>
        /// <param name="item">The user to be created</param>
        /// <returns>ID of the created User</returns>
        int RegisterNewUser(UserItem item);

        /// <summary>
        /// Returns a list of all Manager-type users from the system for Admin use
        /// </summary>
        /// <returns>List of Mangaer users</returns>
        IList<UserItem> GetAllManagers();

        /// <summary>
        /// Returns a list of all Salesperson-type users from the system for a particular Manager
        /// </summary>
        /// <param name="managerID">Database ID of the Manager</param>
        /// <returns>List of Salespeople under the given Manager</returns>
        IList<UserItem> GetMySalespeople(int managerID);

        /// <summary>
        /// Returns a Select List of all Salesperson-type users from the system for a particular Manager
        /// </summary>
        /// <param name="managerID">Database ID of the Manager</param>
        /// <param name="salespersonID">Database ID of the Salesperson currently assigned to the House</param>
        /// <returns>Select List of Salespeople under the given Manager</returns>
        IList<SelectListItem> GetMySalespeopleOptions(int managerID, int assignedSalespersonID = 0);

        /// <summary>
        /// Set's the user's Reset Password flag. Throws error if unsuccessful
        /// </summary>
        /// <param name="userId">User's Database ID</param>
        void MarkResetPassword(int userId, string newPassword);

        /// <summary>
        /// Reset's the given User's Password values
        /// </summary>
        /// <param name="emailAddress">Email Address of the User</param>
        /// <param name="salt">New Salt value for the User</param>
        /// <param name="hash">New Hash value for the User</param>
        /// <returns>True if successful, false if unsuccessful</returns>
        bool ResetPassword(string emailAddress, string salt, string hash);

        /// <summary>
        /// Pairs the current logged in Manager with the newly created Salesperson
        /// </summary>
        /// <param name="managerID">User ID of the Manager</param>
        /// <param name="SalespersonID">User ID of the Salesperson</param>
        void PairManagerWithSalesperson(int managerID, int SalespersonID);

        /// <summary>
        /// Updates the User's Profile
        /// </summary>
        /// <param name="userID">Database ID of the User</param>
        /// <param name="emailAddress">User's new Email Address</param>
        /// <param name="firstName">User's new First Name</param>
        /// <param name="lastName">User's new Last Name</param>
        void UpdateProfile(int userID, string emailAddress, string firstName, string lastName);

        #endregion

        #region Houses Methods

        /// <summary>
        /// Returns a list of Houses associated to the given Manager
        /// </summary>
        /// <param name="managerID">User ID of the Manager</param>
        /// <returns>List of Houses associated to that Manager</returns>
        IList<HouseItem> GetAllHouses(int managerID);

        /// <summary>
        /// Returns a list of Houses associated to the given Salesperson
        /// </summary>
        /// <param name="managerID">User ID of the Manager</param>
        /// <returns>List of Houses associated to that Manager</returns>
        IList<HouseItem> GetAassignedHouses(int salespersonID);

        /// <summary>
        /// Retrieves a specific House from the Database
        /// </summary>
        /// <param name="houseID">Database ID of the House</param>
        /// <returns>HouseItem containing the House's information</returns>
        HouseItem GetHouse(int houseID);

        /// <summary>
        /// Creates a new House in the database
        /// </summary>
        /// <param name="item">The House to be created</param>
        /// <returns>ID of the created House</returns>
        int CreateHouse(HouseItem item);

        /// <summary>
        /// Returns a list of Notes associated with the given House
        /// </summary>
        /// <param name="houseID">Database ID of the House</param>
        /// <returns>List of NoteItems associated with that House</returns>
        IList<NoteItem> GetHouseNotes(int houseID);

        /// <summary>
        /// Creates a Note for a House
        /// </summary>
        /// <param name="note">The Note to be created</param>
        /// <returns>Database ID of the Note</returns>
        int AddHouseNote(NoteItem note);

        /// <summary>
        /// Returns a Select List of all House Statuses from the system
        /// </summary>
        /// <param name="currentStatus">Status of a House to default as Selected</param>
        /// <returns>Select List of Products under the given Salesperson's Manager</returns>
        IList<SelectListItem> GetHouseStatusOptions(int statusID);

        /// <summary>
        /// Changes the given House's Status to the supplied Status
        /// </summary>
        /// <param name="houseID">Database ID of the House to change</param>
        /// <param name="statusID">Database ID of the desired Status</param>
        /// <param name="userID">ID of the requesting User to determine if they are connected to the House</param>
        /// <returns>True if successful, false if failed</returns>
        bool SetHouseStatus(int houseID, int statusID, int userID);

        /// <summary>
        /// Changes the given House's Assigned Salesperson to the supplied Salesperson
        /// </summary>
        /// <param name="houseID">Database ID of the House to change</param>
        /// <param name="salespersonID">Database ID of the Salesperson to reassign</param>
        /// <param name="userID">ID of the requesting User to determine if they are the Manager of the Salesperson and the House</param>
        /// <returns>True if successful, false if failed</returns>
        bool ReassignHouseSalesperson(int houseID, int salespersonID, int userID);

        /// <summary>
        /// Returns a Select List of all Houses from the system assigned to the Salesperson
        /// </summary>
        /// <param name="salespersonID">Database ID of the Salesperson making the Transaction</param>
        /// <returns>Select List of Houses assigned the given Salesperson</returns>
        IList<SelectListItem> GetSalesTransactionHouseOptions(int salespersonID);

        #endregion

        #region Resident Methods

        /// <summary>
        /// Returns a list of Residents associated with the given House
        /// </summary>
        /// <param name="houseID">Database ID of the House</param>
        /// <returns>List of ResidentItems associated with that House</returns>
        IList<ResidentItem> GetHouseResidents(int houseID);

        /// <summary>
        /// Creates a Resident for a House
        /// </summary>
        /// <param name="note">The Resident to be created</param>
        /// <returns>Database ID of the Resident</returns>
        int AddHouseResident(ResidentItem resident);

        #endregion

        #region Manager Dashboard Methods

        /// <summary>
        /// Generates the top salesman based on amount of sales for current manager.
        /// </summary>
        /// <param name="managerID"></param>
        /// <returns>A list of SalesmanCountItem</returns>
        IList<SalesmanSalesCountItem> GetTopSalesmenByQuantity(int managerID);

        /// <summary>
        /// Generates the top salesman based on total revenue of sales for current manager.
        /// </summary>
        /// <param name="managerID"></param>
        /// <returns>A list of SalesRevenueItem</returns>
        IList<SalesmanRevenueItem> GetTopSalesmenByRevenue(int managerID);

        IList<HouseSalesCountItem> GetTopHouseByQuantity(int managerID);

        /// <summary>
        /// Generates the top house based on total revenue of sales for current manager.
        /// </summary>
        /// <param name="managerID"></param>
        /// <returns>A list of HouseRevenueItems</returns>
        IList<HouseRevenueItem> GetTopHouseByRevenue(int managerID);

        /// <summary>
        /// Gets the total number of transactions that have taken place under this manager.
        /// </summary>
        /// <param name="managerID"></param>
        /// <returns>The total number of transactions from a manager's salesmen as an Int</returns>
        int GetManagerTotalSales(int managerID);

        /// <summary>
        /// Gets the total amount of revenue generated under this manager.
        /// </summary>
        /// <param name="managerID"></param>
        /// <returns>The total revenue from manager as an Int</returns>
        double GetManagerTotalRevenue(int managerID);

        #endregion

        #region Salesman Dashboard Methods

        /// <summary>
        /// Generates a list of houses that salesman has assigned.
        /// </summary>
        /// <param name="managerID"></param>
        IList<HouseDashboardItem> GetSalesmanDashboardHouses(int salesmanID);

        /// <summary>
        /// Generates a list of products the salesman has avaliable.
        /// </summary>
        /// <param name="managerID"></param>
        IList<ProductDashboardItem> GetSalesmanDashboardProducts(int salesmanID);

        /// <summary>
        /// Gets the total number of transactions that have taken place from this Salesman.
        /// </summary>
        /// <param name="managerID"></param>
        /// <returns>The total number of transactions from a salesmen as an Int</returns>
        int GetSalesmanDashboardSales(int salesmanID);

        /// <summary>
        /// Gets the total amount of revenue generated from Salesman.
        /// </summary>
        /// <param name="managerID"></param>
        /// <returns>The total revenue from Salesman as an Int</returns>
        double GetSalesmanDashboardTotalRevenue(int salemanID);

        /// <summary>
        /// Retrieves the top five Houses by number of Sales Transactions for a particular Salesperson
        /// </summary>
        /// <param name="salespersonID">Database ID of the Salesperson</param>
        /// <returns>List containing top five SalespersonBestCustomerCountItem for the Salesperson</returns>
        IList<SalespersonBestCustomerCountItem> GetSalesmanDashboardCount(int salespersonID);

        /// <summary>
        /// Retrieves the top five Houses by Revenue for a particular Salesperson
        /// </summary>
        /// <param name="salespersonID">Database ID of the Salesperson</param>
        /// <returns>List containing top five SalespersonBestCustomerRevenueItem for the Salesperson</returns>
        IList<SalespersonBestCustomerRevenueItem> GetSalesmanDashboardRevenue(int salespersonID);

        #endregion

        #region Product Methods

        /// <summary>
        /// Retrieves a list of Products associated with the given Manager
        /// </summary>
        /// <param name="managerID">Database ID of the Manager</param>
        /// <returns>List of Products associated with the manager</returns>
        IList<ProductItem> GetMyProducts(int managerID);

        /// <summary>
        /// Creates a Product in the Database with the given name and associates that Product with the given Manager
        /// </summary>
        /// <param name="productName">Name of the new Product</param>
        /// <param name="managerID">Database ID of the Manager</param>
        /// <returns>Newly created Product's ID</returns>
        int CreateProduct(string productName, int managerID);

        /// <summary>
        /// Returns a Select List of all Products from the system created by the Salesperson's Manager
        /// </summary>
        /// <param name="salespersonID">Database ID of the Salesperson</param>
        /// <returns>Select List of Products under the given Salesperson's Manager</returns>
        IList<SelectListItem> GetMyProductOptions(int salespersonID);

        /// <summary>
        /// Removes the Product from the Manager's list
        /// </summary>
        /// <param name="productID">Product's Database ID</param>
        /// <param name="managerID">Manager the Product belongs to</param>
        void RemoveProduct(int productID, int managerID);

        #endregion

        #region Transaction Methods

        /// <summary>
        /// Generates a list of a salesman's transactions
        /// </summary>
        /// <param name="managerID"></param>
        /// <returns>A list of SalesTransactionItems</returns>
        IList<TransactionItem> GetTransactions(int salesmanID);

        /// <summary>
        /// Creates a transaction for a salesman
        /// </summary>
        /// <param name="note">The transaction to be created</param>
        /// <returns>Database ID of the transaction</returns>
        int AddTransaction(SalesTransactionItem transaction);

        /// <summary>
        /// Generates a Report (List of ReportItems) for a Manager
        /// </summary>
        /// <param name="managerID">Database ID of the Manager</param>
        /// <returns>List of ReportItems</returns>
        IList<ReportItem> GetReport(int managerID);

        #endregion
    }
}
