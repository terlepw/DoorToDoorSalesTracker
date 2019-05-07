using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DoorToDoorLibrary.Exceptions;
using DoorToDoorLibrary.DatabaseObjects;
using Microsoft.AspNetCore.Mvc.Rendering;
using DoorToDoorLibrary.Logic;

namespace DoorToDoorLibrary.DAL
{
    public class DoorToDoorDAL : IDoorToDoorDAL
    {
        #region Properties and Variables

        private string _connectionString;

        private const string _getLastIdSql = "SELECT CAST(SCOPE_IDENTITY() AS int);";

        #endregion

        #region Constructor

        public DoorToDoorDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion

        #region User Methods

        /// <summary>
        /// Finds a single user in the database using the user's Email Address
        /// </summary>
        /// <param name="emailAddress">The desired user's Email Address</param>
        /// <returns>UserItem containing the user's information</returns>
        public UserItem GetUserItem(string emailAddress)
        {
            UserItem user = null;
            const string sql = "SELECT * From [Users] WHERE emailAddress = @EmailAddress;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@EmailAddress", emailAddress.ToLower());

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    user = GetUserItemFromReader(reader);
                }
            }

            if (user == null)
            {
                throw new Exception("User does not exist.");
            }

            return user;
        }

        /// <summary>
        /// Generates a UserItem from the provided Sql Data Reader
        /// </summary>
        /// <param name="reader">The given Sql Data Reader</param>
        /// <returns>UserItem containing the information for a particular user</returns>
        private UserItem GetUserItemFromReader(SqlDataReader reader)
        {
            UserItem item = new UserItem();

            item.Id = Convert.ToInt32(reader["id"]);
            item.FirstName = Convert.ToString(reader["firstName"]);
            item.LastName = Convert.ToString(reader["lastName"]);
            item.EmailAddress = Convert.ToString(reader["emailAddress"]);
            item.Hash = Convert.ToString(reader["hash"]);
            item.Salt = Convert.ToString(reader["salt"]);
            item.RoleId = Convert.ToInt32(reader["roleID"]);
            item.UpdatePassword = Convert.ToBoolean(reader["updatePassword"]);

            return item;
        }

        /// <summary>
        /// Creates a new User in the database
        /// </summary>
        /// <param name="item">The user to be created</param>
        /// <returns>ID of the created User</returns>
        public int RegisterNewUser(UserItem item)
        {
            int ID = 0;

            const string sql = "INSERT INTO [Users] (firstName, lastName, emailAddress, hash, salt, roleID, updatePassword) " +
                               "VALUES (@FirstName, @LastName, @EmailAddress, @Hash, @Salt, @RoleId, 1);";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + " " + _getLastIdSql, conn);
                cmd.Parameters.AddWithValue("@FirstName", item.FirstName);
                cmd.Parameters.AddWithValue("@LastName", item.LastName);
                cmd.Parameters.AddWithValue("@EmailAddress", item.EmailAddress.ToLower());
                cmd.Parameters.AddWithValue("@Hash", item.Hash);
                cmd.Parameters.AddWithValue("@Salt", item.Salt);
                cmd.Parameters.AddWithValue("@RoleId", item.RoleId);

                try
                {
                    ID = (int)cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return ID;
        }

        /// <summary>
        /// Returns a list of all Manager-type users from the system for Admin use
        /// </summary>
        /// <returns>List of Mangaer users</returns>
        public IList<UserItem> GetAllManagers()
        {
            List<UserItem> output = new List<UserItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM [Users] WHERE roleID = (SELECT id FROM Roles WHERE [name] = 'Manager') ORDER BY lastName, firstName;";

                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    UserItem newuser = GetUserItemFromReader(reader);
                    output.Add(newuser);
                }
            }

            return output;
        }

        /// <summary>
        /// Returns a list of all Salesperson-type users from the system for a particular Manager
        /// </summary>
        /// <param name="managerID">Database ID of the Manager</param>
        /// <returns>List of Salespeople under the given Manager</returns>
        public IList<UserItem> GetMySalespeople(int managerID)
        {
            List<UserItem> output = new List<UserItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM [Users] WHERE id IN(SELECT salespersonID FROM Manager_Saleperson WHERE managerID = @ManagerID)" +
                    " ORDER BY lastName, firstName;";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ManagerID", managerID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    UserItem newuser = GetUserItemFromReader(reader);
                    output.Add(newuser);
                }
            }

            return output;
        }

        /// <summary>
        /// Returns a Select List of all Salesperson-type users from the system for a particular Manager
        /// </summary>
        /// <param name="managerID">Database ID of the Manager</param>
        /// <param name="salespersonID">Database ID of the Salesperson currently assigned to the House</param>
        /// <returns>Select List of Salespeople under the given Manager</returns>
        public IList<SelectListItem> GetMySalespeopleOptions(int managerID, int assignedSalespersonID = 0)
        {
            List<SelectListItem> output = new List<SelectListItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT firstName, lastName, [id] FROM [Users] WHERE id IN(SELECT salespersonID FROM Manager_Saleperson" +
                    " WHERE managerID = @ManagerID) ORDER BY lastName, firstName;";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ManagerID", managerID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string salespersonName = Convert.ToString(reader["firstName"]) + " " + Convert.ToString(reader["lastName"]);
                    int salespersonID = Convert.ToInt32(reader["id"]);
                    SelectListItem item = new SelectListItem(salespersonName, salespersonID.ToString(), (salespersonID == assignedSalespersonID));

                    output.Add(item);
                }
            }

            return output;
        }

        /// <summary>
        /// Set's the user's Reset Password flag. Throws error if unsuccessful
        /// </summary>
        /// <param name="userId">User's Database ID</param>
        public void MarkResetPassword(int userId, string newPassword)
        {
            int numRows = 0;

            PasswordManager pm = new PasswordManager(newPassword);
            string salt = pm.Salt;
            string hash = pm.Hash;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "UPDATE Users SET updatePassword = 1, salt = @Salt, hash = @Hash WHERE id = @UserID;";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@Salt", salt);
                cmd.Parameters.AddWithValue("@Hash", hash);

                numRows = cmd.ExecuteNonQuery();
            }

            if (numRows != 1)
            {
                throw new MarkResetPasswordFailedException();
            }
        }

        /// <summary>
        /// Reset's the given User's Password values
        /// </summary>
        /// <param name="emailAddress">Email Address of the User</param>
        /// <param name="salt">New Salt value for the User</param>
        /// <param name="hash">New Hash value for the User</param>
        /// <returns>True if successful, false if unsuccessful</returns>
        public bool ResetPassword(string emailAddress, string salt, string hash)
        {
            int numRows = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "UPDATE Users SET salt = @Salt, hash = @Hash, updatePassword = 0 WHERE emailAddress = @EmailAddress;";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Salt", salt);
                cmd.Parameters.AddWithValue("@Hash", hash);
                cmd.Parameters.AddWithValue("@EmailAddress", emailAddress.ToLower());

                numRows = cmd.ExecuteNonQuery();
            }

            return numRows == 1 ? true : false;
        }

        /// <summary>
        /// Pairs the current logged in Manager with the newly created Salesperson
        /// </summary>
        /// <param name="managerID">User ID of the Manager</param>
        /// <param name="SalespersonID">User ID of the Salesperson</param>
        public void PairManagerWithSalesperson(int managerID, int SalespersonID)
        {
            int numRows = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "INSERT INTO [Manager_Saleperson] (managerID, salespersonID) VALUES (@ManagerID, @SalespersonID);";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ManagerID", managerID);
                cmd.Parameters.AddWithValue("@SalespersonID", SalespersonID);

                numRows = cmd.ExecuteNonQuery();
            }

            if (numRows != 1)
            {
                throw new ManagerSalespersonLinkFailedException();
            }
        }

        private bool IsMySalesperson(int salespersonID, int managerID)
        {
            foreach (UserItem user in GetMySalespeople(managerID))
            {
                if (user.Id == salespersonID)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Updates the User's Profile
        /// </summary>
        /// <param name="userID">Database ID of the User</param>
        /// <param name="emailAddress">User's new Email Address</param>
        /// <param name="firstName">User's new First Name</param>
        /// <param name="lastName">User's new Last Name</param>
        public void UpdateProfile(int userID, string emailAddress, string firstName, string lastName)
        {
            int numRows = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "UPDATE Users SET emailAddress = @EmailAddress, firstName = @FirstName, lastName = @LastName WHERE id = @UserID;";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);

                numRows = cmd.ExecuteNonQuery();
            }

            if (numRows != 1)
            {
                throw new ProfileUpdateFailedException();
            }
        }

        #endregion

        #region House Methods

        /// <summary>
        /// Returns a list of Houses associated to the given Manager
        /// </summary>
        /// <param name="managerID">User ID of the Manager</param>
        /// <returns>List of Houses associated to that Manager</returns>
        public IList<HouseItem> GetAllHouses(int managerID)
        {
            List<HouseItem> houseList = new List<HouseItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT h.*, (u.firstName + ' ' + u.lastName) AS salespersonName " +
                    "FROM [Houses] AS h JOIN [Users] AS u ON h.salespersonID = u.id " +
                    "WHERE h.managerID = @ManagerID ORDER BY country, district, city, street;";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ManagerID", managerID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    HouseItem newHouse = GetHouseItemFromReader(reader);
                    houseList.Add(newHouse);
                }
            }

            return houseList;
        }

        /// <summary>
        /// Returns a list of Houses associated to the given Salesperson
        /// </summary>
        /// <param name="managerID">User ID of the Manager</param>
        /// <returns>List of Houses associated to that Manager</returns>
        public IList<HouseItem> GetAassignedHouses(int salespersonID)
        {
            List<HouseItem> houseList = new List<HouseItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT h.*, (u.firstName + ' ' + u.lastName) AS salespersonName " +
                    "FROM [Houses] AS h JOIN [Users] AS u ON h.salespersonID = u.id " +
                    "WHERE h.salespersonID = @SalespersonID ORDER BY country, district, city, street;";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@SalespersonID", salespersonID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    HouseItem newHouse = GetHouseItemFromReader(reader);
                    houseList.Add(newHouse);
                }
            }

            return houseList;
        }

        /// <summary>
        /// Retrieves a specific House from the Database
        /// </summary>
        /// <param name="houseID">Database ID of the House</param>
        /// <returns>HouseItem containing the House's information</returns>
        public HouseItem GetHouse(int houseID)
        {
            HouseItem house = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT h.*, (u.firstName + ' ' + u.lastName) AS salespersonName " +
                    "FROM[Houses] AS h JOIN[Users] AS u ON h.salespersonID = u.id " +
                    "WHERE h.id = @HouseID;";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@HouseID", houseID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    house = GetHouseItemFromReader(reader);
                }
            }

            return house;
        }

        /// <summary>
        /// Generates a HouseItem from the provided Sql Data Reader
        /// </summary>
        /// <param name="reader">The given Sql Data Reader</param>
        /// <returns>HouseItem containing the information for a particular house</returns>
        private HouseItem GetHouseItemFromReader(SqlDataReader reader)
        {
            HouseItem item = new HouseItem();

            item.Id = Convert.ToInt32(reader["id"]);
            item.Street = Convert.ToString(reader["street"]);
            item.City = Convert.ToString(reader["city"]);
            item.District = Convert.ToString(reader["district"]);
            item.ZipCode = Convert.ToString(reader["zipCode"]);
            item.Country = Convert.ToString(reader["country"]);
            item.ManagerID = Convert.ToInt32(reader["managerID"]);
            item.AssignedSalespersonID = Convert.ToInt32(reader["salespersonID"]);
            item.StatusID = Convert.ToInt32(reader["statusID"]);
            item.AssignedSalesperson = Convert.ToString(reader["salespersonName"]);

            return item;
        }

        /// <summary>
        /// Creates a new House in the database
        /// </summary>
        /// <param name="item">The House to be created</param>
        /// <returns>ID of the created House</returns>
        public int CreateHouse(HouseItem item)
        {
            if (IsMySalesperson(item.AssignedSalespersonID, item.ManagerID))
            {
                int ID = 0;

                const string sql = "INSERT INTO [Houses] (street, city, district, zipCode, country, managerID, salespersonID, statusID) " +
                                   "VALUES (@Street, @City, @District, @ZipCode, @Country, @ManagerID, @SalespersonID, 1);";
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql + " " + _getLastIdSql, conn);
                    cmd.Parameters.AddWithValue("@Street", item.Street.ToLower());
                    cmd.Parameters.AddWithValue("@City", item.City.ToLower());
                    cmd.Parameters.AddWithValue("@District", item.District.ToLower());
                    cmd.Parameters.AddWithValue("@ZipCode", item.ZipCode.ToLower());
                    cmd.Parameters.AddWithValue("@Country", item.Country.ToLower());
                    cmd.Parameters.AddWithValue("@ManagerID", item.ManagerID);
                    cmd.Parameters.AddWithValue("@SalespersonID", item.AssignedSalespersonID);

                    try
                    {
                        ID = (int)cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                return ID;
            }
            else
            {
                throw new NotMySalespersonException();
            }
        }

        /// <summary>
        /// Returns a list of Notes associated with the given House
        /// </summary>
        /// <param name="houseID">Database ID of the House</param>
        /// <returns>List of NoteItems associated with that House</returns>
        public IList<NoteItem> GetHouseNotes(int houseID)
        {
            List<NoteItem> notes = new List<NoteItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT hn.*, (u.firstName + ' ' + u.lastName) AS userName FROM [Houses_Notes] AS hn " +
                    "JOIN [Users] AS u ON hn.userID = u.id WHERE houseID = @HouseID ORDER BY [date] DESC;";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@HouseID", houseID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    NoteItem newNote = GetNoteItemFromReader(reader);
                    notes.Add(newNote);
                }
            }

            return notes;
        }

        /// <summary>
        /// Generates a NoteItem from the provided Sql Data Reader
        /// </summary>
        /// <param name="reader">The given Sql Data Reader</param>
        /// <returns>NoteItem containing the information for a particular Note</returns>
        private NoteItem GetNoteItemFromReader(SqlDataReader reader)
        {
            NoteItem item = new NoteItem();

            item.Id = Convert.ToInt32(reader["id"]);
            item.HouseID = Convert.ToInt32(reader["houseID"]);
            item.UserID = Convert.ToInt32(reader["userID"]);
            item.SubmittedDate = Convert.ToDateTime(reader["date"]);
            item.Note = Convert.ToString(reader["note"]);
            item.UserName = Convert.ToString(reader["userName"]);

            return item;
        }

        /// <summary>
        /// Creates a Note for a House
        /// </summary>
        /// <param name="note">The Note to be created</param>
        /// <returns>Database ID of the Note</returns>
        public int AddHouseNote(NoteItem note)
        {
            if (IsMyHouse(note.UserID, note.HouseID))
            {
                int ID = 0;

                const string sql = "INSERT INTO [Houses_Notes] (houseID, userID, date, note) " +
                                   "VALUES (@HouseID, @UserID, @Date, @Note);";
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(sql + " " + _getLastIdSql, conn);
                    cmd.Parameters.AddWithValue("@HouseID", note.HouseID);
                    cmd.Parameters.AddWithValue("@UserID", note.UserID);
                    cmd.Parameters.AddWithValue("@Date", note.SubmittedDate);
                    cmd.Parameters.AddWithValue("@Note", note.Note);

                    try
                    {
                        ID = (int)cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                return ID;
            }
            else
            {
                throw new NotMyHouseException();
            }
        }

        private bool IsMyHouse(int userID, int houseID)
        {
            HouseItem house = GetHouse(houseID);

            return ((userID == house.ManagerID) || (userID == house.AssignedSalespersonID));
        }

        /// <summary>
        /// Returns a Select List of all House Statuses from the system
        /// </summary>
        /// <param name="currentStatus">Status of a House to default as Selected</param>
        /// <returns>Select List of Products under the given Salesperson's Manager</returns>
        public IList<SelectListItem> GetHouseStatusOptions(int currentStatus)
        {
            List<SelectListItem> output = new List<SelectListItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM House_Status;";

                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string status = Convert.ToString(reader["status"]);
                    int statusID = Convert.ToInt32(reader["id"]);
                    SelectListItem item = new SelectListItem(status, statusID.ToString(), (statusID == currentStatus));

                    output.Add(item);
                }
            }

            return output;
        }

        /// <summary>
        /// Changes the given House's Status to the supplied Status
        /// </summary>
        /// <param name="houseID">Database ID of the House to change</param>
        /// <param name="statusID">Database ID of the desired Status</param>
        /// <param name="userID">ID of the requesting User to determine if they are connected to the House</param>
        /// <returns>True if successful, false if failed</returns>
        public bool SetHouseStatus(int houseID, int statusID, int userID)
        {
            if (IsMyHouse(userID, houseID))
            {
                int numRows = 0;

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    string sql = "UPDATE Houses SET statusID = @StatusID WHERE [id] = @HouseID;";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@StatusID", statusID);
                    cmd.Parameters.AddWithValue("@HouseID", houseID);

                    numRows = cmd.ExecuteNonQuery();
                }

                return numRows == 1 ? true : false;
            }
            else
            {
                throw new NotMyHouseException();
            }
        }

        /// <summary>
        /// Changes the given House's Assigned Salesperson to the supplied Salesperson
        /// </summary>
        /// <param name="houseID">Database ID of the House to change</param>
        /// <param name="salespersonID">Database ID of the Salesperson to reassign</param>
        /// <param name="userID">ID of the requesting User to determine if they are the Manager of the Salesperson and the House</param>
        /// <returns>True if successful, false if failed</returns>
        public bool ReassignHouseSalesperson(int houseID, int salespersonID, int userID)
        {
            if (IsMyHouse(userID, houseID) && IsMySalesperson(salespersonID, userID))
            {
                int numRows = 0;

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    string sql = "UPDATE Houses SET salespersonID = @SalespersonID WHERE [id] = @HouseID;";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@SalespersonID", salespersonID);
                    cmd.Parameters.AddWithValue("@HouseID", houseID);

                    numRows = cmd.ExecuteNonQuery();
                }

                return numRows == 1 ? true : false;
            }
            else
            {
                if(!IsMyHouse(userID, houseID))
                {
                    throw new NotMyHouseException();
                }
                else
                {
                    throw new NotMySalespersonException();
                }
            }
        }

        /// <summary>
        /// Returns a Select List of all Houses from the system assigned to the Salesperson
        /// </summary>
        /// <param name="salespersonID">Database ID of the Salesperson making the Transaction</param>
        /// <returns>Select List of Houses assigned the given Salesperson</returns>
        public IList<SelectListItem> GetSalesTransactionHouseOptions(int salespersonID)
        {
            List<SelectListItem> output = new List<SelectListItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM Houses WHERE salespersonID = @SalespersonID ORDER BY country, district, city, street;";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@SalespersonID", salespersonID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string street = Convert.ToString(reader["street"]);
                    string city = Convert.ToString(reader["city"]);
                    string district = Convert.ToString(reader["district"]);
                    string zipCode = Convert.ToString(reader["zipCode"]);
                    string country = Convert.ToString(reader["country"]);
                    string fullAddress = $"{street}, {city}, {district} {zipCode}, {country}";
                    int houseID = Convert.ToInt32(reader["id"]);
                    SelectListItem item = new SelectListItem(fullAddress, houseID.ToString());

                    output.Add(item);
                }
            }

            return output;
        }

        #endregion

        #region Resident Methods

        /// <summary>
        /// Returns a list of Residents associated with the given House
        /// </summary>
        /// <param name="houseID">Database ID of the House</param>
        /// <returns>List of ResidentItems associated with that House</returns>
        public IList<ResidentItem> GetHouseResidents(int houseID)
        {
            List<ResidentItem> residents = new List<ResidentItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM [Residents] WHERE houseID = @HouseID ORDER BY lastName, firstName;";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@HouseID", houseID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ResidentItem newResident = GetResidentItemFromReader(reader);
                    residents.Add(newResident);
                }
            }

            return residents;
        }

        /// <summary>
        /// Generates a ResidentItem from the provided Sql Data Reader
        /// </summary>
        /// <param name="reader">The given Sql Data Reader</param>
        /// <returns>ResidentItem containing the information for a particular Resident</returns>
        private ResidentItem GetResidentItemFromReader(SqlDataReader reader)
        {
            ResidentItem item = new ResidentItem();

            item.Id = Convert.ToInt32(reader["id"]);
            item.HouseID = Convert.ToInt32(reader["houseID"]);
            item.FirstName = Convert.ToString(reader["firstName"]);
            item.LastName = Convert.ToString(reader["lastName"]);

            return item;
        }

        /// <summary>
        /// Creates a Resident for a House
        /// </summary>
        /// <param name="note">The Resident to be created</param>
        /// <returns>Database ID of the Resident</returns>
        public int AddHouseResident(ResidentItem resident)
        {
            int ID = 0;

            const string sql = "INSERT INTO [Residents] (houseID, firstName, lastName) " +
                                   "VALUES (@HouseID, @FirstName, @LastName);";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + " " + _getLastIdSql, conn);
                cmd.Parameters.AddWithValue("@HouseID", resident.HouseID);
                cmd.Parameters.AddWithValue("@FirstName", resident.FirstName);
                cmd.Parameters.AddWithValue("@LastName", resident.LastName);

                try
                {
                    ID = (int)cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return ID;
        }

        #endregion

        #region Manager Dashboard Methods

        /// <summary>
        /// Generates a SalesmanSalesCountItem from the provided Sql Data Reader
        /// </summary>
        /// <param name="reader">The given Sql Data Reader</param>
        /// <returns>SalesmanSalesCountItem containing the information for a particular sale</returns>
        private SalesmanSalesCountItem GetSalesCountItemFromReader(SqlDataReader reader)
        {
            SalesmanSalesCountItem item = new SalesmanSalesCountItem();

            item.FirstName = Convert.ToString(reader["firstName"]);
            item.LastName = Convert.ToString(reader["lastName"]);
            item.SalesCount = Convert.ToInt32(reader["numSales"]);


            return item;
        }

        /// <summary>
        /// Generates the top salesman based on amount of sales for current manager.
        /// </summary>
        /// <param name="managerID"></param>
        /// <returns>A list of SalesmanCountItem</returns>
        public IList<SalesmanSalesCountItem> GetTopSalesmenByQuantity(int managerID)
        {
            var output = new List<SalesmanSalesCountItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT TOP 5 u.firstName, u.lastName, COUNT(st.ID) AS numSales " +
                    "FROM Users AS u JOIN Sales_Transactions AS st ON u.id = st.salespersonID WHERE u.id " +
                    "IN(SELECT ms.salespersonID FROM Manager_Saleperson AS ms WHERE ms.managerID = @ManagerID) " +
                    "GROUP BY u.firstName, u.lastName ORDER BY numSales DESC; ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ManagerID", managerID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    SalesmanSalesCountItem sale = GetSalesCountItemFromReader(reader);
                    output.Add(sale);
                }
            }
            return output;
        }

        /// <summary>
        /// Generates a SalesmanRevenueItem from the provided Sql Data Reader
        /// </summary>
        /// <param name="reader">The given Sql Data Reader</param>
        /// <returns>SalesmanRevenueItem containing the information for a particular sale</returns>
        private SalesmanRevenueItem GetSalesRevenueItemFromReader(SqlDataReader reader)
        {
            SalesmanRevenueItem item = new SalesmanRevenueItem();

            item.FirstName = Convert.ToString(reader["firstName"]);
            item.LastName = Convert.ToString(reader["lastName"]);
            item.TotalRevenue = Convert.ToInt32(reader["numTotal"]);


            return item;
        }

        /// <summary>
        /// Generates the top salesman based on total revenue of sales for current manager.
        /// </summary>
        /// <param name="managerID"></param>
        /// <returns>A list of SalesRevenueItem</returns>
        public IList<SalesmanRevenueItem> GetTopSalesmenByRevenue(int managerID)
        {
            var output = new List<SalesmanRevenueItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT TOP 5 u.firstName, u.lastName, SUM(st.amount) AS numTotal " +
                    "FROM Users AS u JOIN Sales_Transactions AS st ON u.id = st.salespersonID WHERE u.id " +
                    "IN(SELECT ms.salespersonID FROM Manager_Saleperson AS ms WHERE ms.managerID = @ManagerID) " +
                    "GROUP BY u.firstName, u.lastName ORDER BY numTotal DESC; ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ManagerID", managerID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    SalesmanRevenueItem sale = GetSalesRevenueItemFromReader(reader);
                    output.Add(sale);
                }
            }

            return output;
        }

        /// <summary>
        /// Generates a HouseSalesCount from the provided Sql Data Reader
        /// </summary>
        /// <param name="reader">The given Sql Data Reader</param>
        /// <returns>HouseSalesCount containing the information for a particular sale</returns>
        private HouseSalesCountItem GetHouseSalesCountItemFromReader(SqlDataReader reader)
        {
            HouseSalesCountItem item = new HouseSalesCountItem();

            item.Street = Convert.ToString(reader["street"]);
            item.City = Convert.ToString(reader["city"]);
            item.District = Convert.ToString(reader["district"]);
            item.Country = Convert.ToString(reader["country"]);
            item.SalesCount = Convert.ToInt32(reader["numSales"]);

            return item;
        }

        /// <summary>
        /// Generates the top house based on amount of sales for current manager.
        /// </summary>
        /// <param name="managerID"></param>
        /// <returns>A list of HouseSalesCountItems</returns>
        public IList<HouseSalesCountItem> GetTopHouseByQuantity(int managerID)
        {
            var output = new List<HouseSalesCountItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT TOP 5 h.street, h.city, h.district, h.country, COUNT(st.ID) as numSales " +
                    "FROM Houses AS h JOIN Sales_Transactions AS st ON h.id = st.houseID WHERE h.managerID = @ManagerID " +
                    "GROUP BY h.street, h.city, h.district, h.country ORDER BY numSales DESC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ManagerID", managerID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    HouseSalesCountItem sale = GetHouseSalesCountItemFromReader(reader);
                    output.Add(sale);
                }
            }

            return output;
        }

        /// <summary>
        /// Generates a HouseRevenue from the provided Sql Data Reader
        /// </summary>
        /// <param name="reader">The given Sql Data Reader</param>
        /// <returns>HouseRevenue containing the information for a particular sale</returns>
        private HouseRevenueItem GetHouseRevenueItemFromReader(SqlDataReader reader)
        {
            HouseRevenueItem item = new HouseRevenueItem();

            item.Street = Convert.ToString(reader["street"]);
            item.City = Convert.ToString(reader["city"]);
            item.District = Convert.ToString(reader["district"]);
            item.Country = Convert.ToString(reader["country"]);
            item.TotalRevenue = Convert.ToInt32(reader["numTotal"]);

            return item;
        }

        /// <summary>
        /// Generates the top house based on total revenue of sales for current manager.
        /// </summary>
        /// <param name="managerID"></param>
        /// <returns>A list of HouseRevenueItems</returns>
        public IList<HouseRevenueItem> GetTopHouseByRevenue(int managerID)
        {
            var output = new List<HouseRevenueItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT TOP 5 h.street, h.city, h.district, h.country, SUM(st.amount) as numTotal " +
                    "FROM Houses AS h JOIN Sales_Transactions AS st ON h.id = st.houseID WHERE h.managerID = @ManagerID " +
                    "GROUP BY h.street, h.city, h.district, h.country ORDER BY numTotal DESC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ManagerID", managerID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    HouseRevenueItem sale = GetHouseRevenueItemFromReader(reader);
                    output.Add(sale);
                }

            }

            return output;
        }

        /// <summary>
        /// Gets the total number of transactions that have taken place under this manager.
        /// </summary>
        /// <param name="managerID"></param>
        /// <returns>The total number of transactions from a manager's salesmen as an Int</returns>
        public int GetManagerTotalSales(int managerID)
        {
            int output = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT COUNT(st.id) AS salesCount FROM Sales_Transactions AS st " +
                    "JOIN Users AS u ON u.id = st.salespersonID WHERE u.id " +
                    "IN(SELECT ms.salespersonID FROM Manager_Saleperson AS ms WHERE ms.managerID = @ManagerID);";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ManagerID", managerID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    output = Convert.ToInt32(reader["salesCount"]);
                }
            }

            return output;
        }

        /// <summary>
        /// Gets the total amount of revenue generated under this manager.
        /// </summary>
        /// <param name="managerID"></param>
        /// <returns>The total revenue from manager as an Int</returns>
        public double GetManagerTotalRevenue(int managerID)
        {
            double output = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT Sum(st.amount) AS revenue FROM Sales_Transactions AS st " +
                    "JOIN Users AS u ON u.id = st.salespersonID WHERE u.id " +
                    "IN(SELECT ms.salespersonID FROM Manager_Saleperson AS ms WHERE ms.managerID = @ManagerID); ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ManagerID", managerID);

                SqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        output = Convert.ToDouble(reader["revenue"]);
                    }
                }
                catch
                {
                    output = 0;
                }
            }


            return output;
        }

        #endregion

        #region Salesman Dashboard Methods

        /// <summary>
        /// Generates a HouseDashboardItem from the provided Sql Data Reader
        /// </summary>
        /// <param name="reader">The given Sql Data Reader</param>
        /// <returns>HouseDashboardItem to be placed into a list</returns>
        private HouseDashboardItem GetHouseDashboardItemFromReader(SqlDataReader reader)
        {
            HouseDashboardItem item = new HouseDashboardItem();

            item.Street = Convert.ToString(reader["street"]);

            return item;
        }

        /// <summary>
        /// Generates a list of houses that salesman has assigned.
        /// </summary>
        /// <param name="managerID"></param>
        public IList<HouseDashboardItem> GetSalesmanDashboardHouses(int salesmanID)
        {
            List<HouseDashboardItem> output = new List<HouseDashboardItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT h.street FROM Houses AS h " +
                    "JOIN Users as u ON u.id = h.salespersonID WHERE u.id = @SalesmanID; ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@SalesmanID", salesmanID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    HouseDashboardItem sale = GetHouseDashboardItemFromReader(reader);
                    output.Add(sale);
                }
            }

            return output;
        }

        /// <summary>
        /// Generates a ProductDashboardItem from the provided Sql Data Reader
        /// </summary>
        /// <param name="reader">The given Sql Data Reader</param>
        /// <returns>ProductDashboardItem to be placed into a list</returns>
        private ProductDashboardItem GetProductDashboardItemFromReader(SqlDataReader reader)
        {
            ProductDashboardItem item = new ProductDashboardItem();

            item.Name = Convert.ToString(reader["name"]);

            return item;
        }

        /// <summary>
        /// Generates a list of products the salesman has avaliable.
        /// </summary>
        /// <param name="managerID"></param>
        public IList<ProductDashboardItem> GetSalesmanDashboardProducts(int salesmanID)
        {
            List<ProductDashboardItem> output = new List<ProductDashboardItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT p.name FROM Products AS p WHERE p.id IN " +
                    "(SELECT mp.productID FROM Manager_Products AS mp WHERE mp.managerID = " +
                    "(SELECT ms.managerID FROM Manager_Saleperson AS ms WHERE ms.salespersonID = @SalesmanID))" +
                    "ORDER BY p.name";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@SalesmanID", salesmanID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ProductDashboardItem sale = GetProductDashboardItemFromReader(reader);
                    output.Add(sale);
                }

                return output;
            }
        }

        /// <summary>
        /// Gets the total number of transactions that have taken place from this Salesman.
        /// </summary>
        /// <param name="managerID"></param>
        /// <returns>The total number of transactions from a salesmen as an Int</returns>
        public int GetSalesmanDashboardSales(int salesmanID)
        {
            int output = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT COUNT(st.id) AS salesCount FROM Sales_Transactions AS st " +
                    "JOIN Users AS u ON u.id = st.salespersonID WHERE u.id = @SalesmanID";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@SalesmanID", salesmanID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    output = Convert.ToInt32(reader["salesCount"]);
                }
            }

            return output;
        }

        /// <summary>
        /// Gets the total amount of revenue generated from Salesman.
        /// </summary>
        /// <param name="managerID"></param>
        /// <returns>The total revenue from Salesman as an Int</returns>
        public double GetSalesmanDashboardTotalRevenue(int salesmanID)
        {
            double output = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT Sum(st.amount) AS revenue FROM Sales_Transactions AS st " +
                    "JOIN Users AS u ON u.id = st.salespersonID WHERE u.id = @SalesmanID";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@SalesmanID", salesmanID);

                SqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        output = Convert.ToDouble(reader["revenue"]);
                    }
                }
                catch
                {
                    output = 0;
                }
            }

            return output;
        }

        /// <summary>
        /// Retrieves the top five Houses by number of Sales Transactions for a particular Salesperson
        /// </summary>
        /// <param name="salespersonID">Database ID of the Salesperson</param>
        /// <returns>List containing top five SalespersonBestCustomerCountItem for the Salesperson</returns>
        public IList<SalespersonBestCustomerCountItem> GetSalesmanDashboardCount(int salespersonID)
        {
            List<SalespersonBestCustomerCountItem> output = new List<SalespersonBestCustomerCountItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT TOP 5 (h.street + ', ' + h.city + ', ' + h.district + ', ' + h.country) AS [address], " +
                    "COUNT(st.[id]) AS transactionCount FROM Houses AS H JOIN Sales_Transactions AS st ON h.[id] = st.houseID " +
                    "WHERE st.salespersonID = @SalesmanID GROUP BY h.id, h.street, h.city, h.district, h.country ORDER BY transactionCount DESC;";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@SalesmanID", salespersonID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    SalespersonBestCustomerCountItem item = GetSalespersonBestCustomerCountItemFromReader(reader);
                    output.Add(item);
                }

                return output;
            }
        }

        /// <summary>
        /// Generates a SalespersonBestCustomerCountItem from the provided Sql Data Reader
        /// </summary>
        /// <param name="reader">The given Sql Data Reader</param>
        /// <returns>SalespersonBestCustomerCountItem to be placed into a list</returns>
        private SalespersonBestCustomerCountItem GetSalespersonBestCustomerCountItemFromReader(SqlDataReader reader)
        {
            SalespersonBestCustomerCountItem item = new SalespersonBestCustomerCountItem();

            item.Address = Convert.ToString(reader["address"]);
            item.TransactionsCount = Convert.ToInt32(reader["transactionCount"]);

            return item;
        }

        /// <summary>
        /// Retrieves the top five Houses by Revenue for a particular Salesperson
        /// </summary>
        /// <param name="salespersonID">Database ID of the Salesperson</param>
        /// <returns>List containing top five SalespersonBestCustomerRevenueItem for the Salesperson</returns>
        public IList<SalespersonBestCustomerRevenueItem> GetSalesmanDashboardRevenue(int salespersonID)
        {
            List<SalespersonBestCustomerRevenueItem> output = new List<SalespersonBestCustomerRevenueItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT TOP 5 (h.street + ', ' + h.city + ', ' + h.district + ', ' + h.country) AS [address], " +
                    "SUM(st.amount) AS totalRevenue FROM Houses AS h JOIN Sales_Transactions AS st ON h.[id] = st.houseID " +
                    "WHERE st.salespersonID = @SalesmanID GROUP BY h.id, h.street, h.city, h.district, h.country ORDER BY totalRevenue DESC;";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@SalesmanID", salespersonID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    SalespersonBestCustomerRevenueItem item = GetSalespersonBestCustomerRevenueItemFromReader(reader);
                    output.Add(item);
                }

                return output;
            }
        }

        /// <summary>
        /// Generates a SalespersonBestCustomerRevenueItem from the provided Sql Data Reader
        /// </summary>
        /// <param name="reader">The given Sql Data Reader</param>
        /// <returns>SalespersonBestCustomerRevenueItem to be placed into a list</returns>
        private SalespersonBestCustomerRevenueItem GetSalespersonBestCustomerRevenueItemFromReader(SqlDataReader reader)
        {
            SalespersonBestCustomerRevenueItem item = new SalespersonBestCustomerRevenueItem();

            item.Address = Convert.ToString(reader["address"]);
            item.TotalRevenue = Convert.ToDouble(reader["totalRevenue"]);

            return item;
        }

        #endregion

        #region Product Methods

        /// <summary>
        /// Retrieves a list of Products associated with the given Manager
        /// </summary>
        /// <param name="managerID">Database ID of the Manager</param>
        /// <returns>List of Products associated with the manager</returns>
        public IList<ProductItem> GetMyProducts(int managerID)
        {
            List<ProductItem> products = new List<ProductItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT * FROM [Products] WHERE id IN(SELECT productID FROM Manager_Products WHERE managerID = @ManagerID) ORDER BY [name];";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ManagerID", managerID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ProductItem product = GetProductItemFromReader(reader);
                    products.Add(product);
                }
            }

            return products;
        }

        /// <summary>
        /// Generates a ProductItem from the provided Sql Data Reader
        /// </summary>
        /// <param name="reader">The given Sql Data Reader</param>
        /// <returns>ProductItem containing the information for a particular product</returns>
        private ProductItem GetProductItemFromReader(SqlDataReader reader)
        {
            ProductItem item = new ProductItem();

            item.Id = Convert.ToInt32(reader["id"]);
            item.Name = Convert.ToString(reader["name"]);

            return item;
        }

        /// <summary>
        /// Creates a Product in the Database with the given name and associates that Product with the given Manager
        /// </summary>
        /// <param name="productName">Name of the new Product</param>
        /// <param name="managerID">Database ID of the Manager</param>
        /// <returns>Newly created Product's ID</returns>
        public int CreateProduct(string productName, int managerID)
        {
            int ID = 0;

            const string sql = "INSERT INTO [Products] (name) VALUES (@Name);";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + " " + _getLastIdSql, conn);
                cmd.Parameters.AddWithValue("@Name", productName.ToLower());

                try
                {
                    ID = (int)cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            LinkProductToManager(ID, managerID);

            return ID;
        }

        /// <summary>
        /// Links the Product with a Manager using their given Database IDs
        /// </summary>
        /// <param name="productName">Name of the Product</param>
        /// <param name="managerID">Database ID of the Manager</param>
        private void LinkProductToManager(int productID, int managerID)
        {
            int numRows = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "INSERT INTO Manager_Products (managerID, productID) VALUES (@ManagerID, @ProductID);";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ManagerID", managerID);
                cmd.Parameters.AddWithValue("@ProductID", productID);

                numRows = cmd.ExecuteNonQuery();
            }

            if (numRows != 1)
            {
                throw new ProductManagerLinkFailedException();
            }
        }

        /// <summary>
        /// Returns a Select List of all Products from the system created by the Salesperson's Manager
        /// </summary>
        /// <param name="salespersonID">Database ID of the Salesperson</param>
        /// <returns>Select List of Products under the given Salesperson's Manager</returns>
        public IList<SelectListItem> GetMyProductOptions(int salespersonID)
        {
            List<SelectListItem> output = new List<SelectListItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT p.* FROM Products AS p WHERE p.id IN (SELECT mp.productID FROM Manager_Products AS mp WHERE mp.managerID = " +
                    "(SELECT ms.managerID FROM Manager_Saleperson AS ms WHERE ms.salespersonID = @SalespersonID)) ORDER BY p.name;";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@SalespersonID", salespersonID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string itemName = Convert.ToString(reader["name"]);
                    int productID = Convert.ToInt32(reader["id"]);
                    SelectListItem item = new SelectListItem(itemName, productID.ToString());

                    output.Add(item);
                }
            }

            return output;
        }

        /// <summary>
        /// Removes the Product from the Manager's list
        /// </summary>
        /// <param name="productID">Product's Database ID</param>
        /// <param name="managerID">Manager the Product belongs to</param>
        public void RemoveProduct(int productID, int managerID)
        {
            int numRows = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "DELETE FROM Manager_Products WHERE managerID = @ManagerID AND productID = @ProductID;";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ManagerID", managerID);
                cmd.Parameters.AddWithValue("@ProductID", productID);

                numRows = cmd.ExecuteNonQuery();
            }

            if (numRows != 1)
            {
                throw new RemoveProductFailedException();
            }
        }

        #endregion

        #region Transaction Methods

        /// <summary>
        /// Generates a SalesTransactionItem from the provided Sql Data Reader
        /// </summary>
        /// <param name="reader">The given Sql Data Reader</param>
        /// <returns>SalesTransactionItem containing the information for a particular sale</returns>
        private TransactionItem GetTransactiontItemFromReader(SqlDataReader reader)
        {
            TransactionItem item = new TransactionItem();

            item.Date = Convert.ToDateTime(reader["date"]);
            item.Amount = Convert.ToDouble(reader["amount"]);
            item.House = Convert.ToString(reader["street"]);
            item.Product = Convert.ToString(reader["name"]);

            return item;
        }

        /// <summary>
        /// Generates a list of a salesman's transactions
        /// </summary>
        /// <param name="managerID"></param>
        /// <returns>A list of SalesTransactionItems</returns>
        public IList<TransactionItem> GetTransactions(int salesmanID)
        {
            List<TransactionItem> output = new List<TransactionItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "	SELECT st.date, st.amount, p.name, h.street FROM Sales_Transactions AS st " +
                    "JOIN Houses AS h ON h.id = st.houseID " +
                    "JOIN Products AS p ON p.id = st.productID " +
                    "WHERE st.salespersonID = @SalespersonID";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@SalespersonID", salesmanID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    TransactionItem sale = GetTransactiontItemFromReader(reader);
                    output.Add(sale);
                }
            }
            return output;
        }

        /// <summary>
        /// Creates a transaction for a salesman
        /// </summary>
        /// <param name="note">The transaction to be created</param>
        /// <returns>Database ID of the transaction</returns>
        public int AddTransaction(SalesTransactionItem transaction)
        {
            int ID = 0;

            const string sql = "INSERT INTO Sales_Transactions (date, amount, houseID, productID, salespersonID) " +
                                "VALUES(CURRENT_TIMESTAMP, @Amount, @HouseID, @ProductID, @SalespersonID);";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql + " " + _getLastIdSql, conn);
                cmd.Parameters.AddWithValue("@HouseID", transaction.HouseID);
                cmd.Parameters.AddWithValue("@SalespersonID", transaction.SalesmanID);
                cmd.Parameters.AddWithValue("@ProductID", transaction.ProductID);
                cmd.Parameters.AddWithValue("@Date", transaction.Date);
                cmd.Parameters.AddWithValue("@Amount", transaction.Amount);

                try
                {
                    ID = (int)cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return ID;
        }

        /// <summary>
        /// Generates a Report (List of ReportItems) for a Manager
        /// </summary>
        /// <param name="managerID">Database ID of the Manager</param>
        /// <returns>List of ReportItems</returns>
        public IList<ReportItem> GetReport(int managerID)
        {
            List<ReportItem> report = new List<ReportItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "SELECT st.[id], (h.street + ', ' + h.city + ', ' + h.district + ', ' + h.zipCode + ', ' + country) AS [address], " +
                    "st.[date], st.amount, p.[name] AS productName, (u.firstName + ' ' + u.lastName) AS[salespersonName] FROM Sales_Transactions AS st " +
                    "JOIN Houses AS h ON st.houseID = h.id JOIN Products AS p ON st.productID = p.id JOIN Users AS u ON st.salespersonID = u.id " +
                    "WHERE st.salespersonID IN(SELECT ms.salespersonID FROM Manager_Saleperson AS ms WHERE ms.managerID = @ManagerID) ORDER BY st.[date] DESC;";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ManagerID", managerID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ReportItem item = GetReportItemFromReader(reader);
                    report.Add(item);
                }
            }

            return report;
        }

        /// <summary>
        /// Generates a Report from the provided Sql Data Reader
        /// </summary>
        /// <param name="reader">The given Sql Data Reader</param>
        /// <returns>Report containing the information for a particular line in a Report</returns>
        private ReportItem GetReportItemFromReader(SqlDataReader reader)
        {
            ReportItem item = new ReportItem();

            item.Id = Convert.ToInt32(reader["id"]);
            item.Address = Convert.ToString(reader["address"]);
            item.Date = Convert.ToDateTime(reader["date"]);
            item.Amount = Convert.ToDouble(reader["amount"]);
            item.ProductName = Convert.ToString(reader["productName"]);
            item.SalespersonName = Convert.ToString(reader["salespersonName"]);

            return item;
        }

        #endregion
    }
}
