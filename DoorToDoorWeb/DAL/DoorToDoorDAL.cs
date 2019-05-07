using System;

namespace DoorToDoorLibrary
{
    public class DoorToDoorDAL
    {
        //#region Properties and Variables
        //private static string connectionString;
        //#endregion

        //#region Constructor
        //public DoorToDoorDAL(string connString)
        //{
        //    connectionString = connString;
        //}
        //#endregion

        //#region Methods

        //public Salesperson MapToPark(SqlDataReader reader)
        //{
        //    return new Park()
        //    {
        //        ParkCode = Convert.ToString(reader["parkCode"]),
        //        ParkName = Convert.ToString(reader["parkName"]),
        //        State = Convert.ToString(reader["state"]),
        //        Acreage = Convert.ToInt32(reader["acreage"]),
        //        ElevationInFeet = Convert.ToInt32(reader["elevationInFeet"]),
        //        MilesOfTrail = Convert.ToDouble(reader["milesOfTrail"]),
        //        NumberOfCampsites = Convert.ToInt32(reader["numberofCampsites"]),
        //        Climate = Convert.ToString(reader["climate"]),
        //        YearFounded = Convert.ToInt32(reader["yearFounded"]),
        //        AnnualVisitorCount = Convert.ToInt32(reader["annualVisitorCount"]),
        //        InspirationalQuote = Convert.ToString(reader["inspirationalQuote"]),
        //        InspirationalQuoteSource = Convert.ToString(reader["inspirationalQuoteSource"]),
        //        ParkDescription = Convert.ToString(reader["parkDescription"]),
        //        EntryFee = Convert.ToInt32(reader["entryFee"]),
        //        NumberOfAnimalSpecies = Convert.ToInt32(reader["numberOfAnimalSpecies"])
        //    };

        //}

        //public static List<Forecast> GetAllForecasts(string parkCode)
        //{
        //    int dayOfTheWeek = 1;
        //    List<Forecast> output = new List<Forecast>();

        //    while (dayOfTheWeek <= 5)
        //    {
        //        output.Add(GetForecast(parkCode, dayOfTheWeek));
        //        dayOfTheWeek++;
        //    }
        //    return output;
        //}

        //public static Forecast GetForecast(string parkCode, int dayOfTheWeek)
        //{
        //    Forecast forecast = new Forecast();
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        conn.Open();
        //        SqlCommand cmd = new SqlCommand("SELECT * FROM weather WHERE parkCode = @parkCode and fiveDayForecastValue = @dayOfWeek", conn);
        //        cmd.Parameters.AddWithValue("@parkCode", parkCode);
        //        cmd.Parameters.AddWithValue("@dayOfWeek", dayOfTheWeek);

        //        SqlDataReader reader = cmd.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            forecast.ParkCode = Convert.ToString(reader["parkCode"]);
        //            forecast.DayOfTheWeek = Convert.ToInt32(reader["fiveDayForecastValue"]);
        //            forecast.LowTemp = Convert.ToInt32(reader["low"]);
        //            forecast.HighTemp = Convert.ToInt32(reader["high"]);
        //            forecast.DailyForecast = Convert.ToString(reader["forecast"]);
        //        }
        //    }
        //    return forecast;
        //}

        //public static List<SelectListItem> GetParkCodeList()
        //{
        //    List<SelectListItem> output = new List<SelectListItem>();
        //    string parkCodeSearch = "select distinct parkCode, parkName from park";

        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        conn.Open();

        //        SqlCommand cmd = new SqlCommand(parkCodeSearch, conn);

        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            var park = new SelectListItem()
        //            {
        //                Text = Convert.ToString(reader["parkName"]),
        //                Value = Convert.ToString(reader["parkCode"])
        //            };
        //            output.Add(park);
        //        }
        //    }

        //    return output;
        //}

        //public static HashSet<SelectListItem> GetAllStates()
        //{
        //    var states = new HashSet<SelectListItem>()
        //{
        //    new SelectListItem() { Value = "AL", Text = "Alabama" },
        //    new SelectListItem() { Value = "AK", Text = "Alaska" },
        //    new SelectListItem() { Value = "AZ", Text = "Arizona" },
        //    new SelectListItem() { Value = "AR", Text = "Arkansas" },
        //    new SelectListItem() { Value = "CA", Text = "California" },
        //    new SelectListItem() { Value = "CO", Text = "Colorado" },
        //    new SelectListItem() { Value = "CT", Text = "Connecticut" },
        //    new SelectListItem() { Value = "DE", Text = "Delaware" },
        //    new SelectListItem() { Value = "DC", Text = "District Of Columbia" },
        //    new SelectListItem() { Value = "FL", Text = "Florida" },
        //    new SelectListItem() { Value = "GA", Text = "Georgia" },
        //    new SelectListItem() { Value = "HI", Text = "Hawaii" },
        //    new SelectListItem() { Value = "ID", Text = "Idaho" },
        //    new SelectListItem() { Value = "IL", Text = "Illinois" },
        //    new SelectListItem() { Value = "IN", Text = "Indiana" },
        //    new SelectListItem() { Value = "IA", Text = "Iowa" },
        //    new SelectListItem() { Value = "KS", Text = "Kansas" },
        //    new SelectListItem() { Value = "KY", Text = "Kentucky" },
        //    new SelectListItem() { Value = "LA", Text = "Louisiana" },
        //    new SelectListItem() { Value = "ME", Text = "Maine" },
        //    new SelectListItem() { Value = "MD", Text = "Maryland" },
        //    new SelectListItem() { Value = "MA", Text = "Massachusetts" },
        //    new SelectListItem() { Value = "MI", Text = "Michigan" },
        //    new SelectListItem() { Value = "MN", Text = "Minnesota" },
        //    new SelectListItem() { Value = "MS", Text = "Mississippi" },
        //    new SelectListItem() { Value = "MO", Text = "Missouri" },
        //    new SelectListItem() { Value = "MT", Text = "Montana" },
        //    new SelectListItem() { Value = "NE", Text = "Nebraska" },
        //    new SelectListItem() { Value = "NV", Text = "Nevada" },
        //    new SelectListItem() { Value = "NH", Text = "New Hampshire" },
        //    new SelectListItem() { Value = "NJ", Text = "New Jersey" },
        //    new SelectListItem() { Value = "NM", Text = "New Mexico" },
        //    new SelectListItem() { Value = "NY", Text = "New York" },
        //    new SelectListItem() { Value = "NC", Text = "North Carolina"},
        //    new SelectListItem() { Value = "ND", Text = "North Dakota"},
        //    new SelectListItem() { Value = "OH", Text = "Ohio" },
        //    new SelectListItem() { Value = "OK", Text = "Oklahoma" },
        //    new SelectListItem() { Value = "OR", Text = "Oregon" },
        //    new SelectListItem() { Value = "PA", Text = "Pennsylvania" },
        //    new SelectListItem() { Value = "RI", Text = "Rhode Island" },
        //    new SelectListItem() { Value = "SC", Text = "South Carolina" },
        //    new SelectListItem() { Value = "SD", Text = "South Dakota" },
        //    new SelectListItem() { Value = "TN", Text = "Tennessee" },
        //    new SelectListItem() { Value = "TX", Text = "Texas" },
        //    new SelectListItem() { Value = "UT", Text = "Utah" },
        //    new SelectListItem() { Value = "VT", Text = "Vermont" },
        //    new SelectListItem() { Value = "VA", Text = "Virginia" },
        //    new SelectListItem() { Value = "WA", Text = "Washington" },
        //    new SelectListItem() { Value = "WV", Text = "West Virginia" },
        //    new SelectListItem() { Value = "WI", Text = "Wisconsin" },
        //    new SelectListItem() { Value = "WY", Text = "Wyoming" }
        //};

        //    return states;
        //}

        //public void SaveNewSurvey(Survey survey)
        //{
        //    // Create a new connection object
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        // Open the connection
        //        conn.Open();
        //        string saveSurvey = "INSERT into survey_result (parkCode,emailAddress,state,activityLevel)" +
        //            "VALUES (@parCode,@emailAddress,@state,@activityLevel)";

        //        SqlCommand cmd = new SqlCommand(saveSurvey, conn);

        //        cmd.Parameters.AddWithValue("@parCode", survey.ParkCode);
        //        cmd.Parameters.AddWithValue("@emailAddress", survey.EmailAddress);
        //        cmd.Parameters.AddWithValue("@state", survey.State);
        //        cmd.Parameters.AddWithValue("@activityLevel", survey.ActivityLevel);

        //        cmd.ExecuteScalar();
        //    }
        //}

        //public List<SurveyResultsViewModel> GetSurveyResults()
        //{
        //    List<SurveyResultsViewModel> output = new List<SurveyResultsViewModel>();

        //    //Create a SqlConnection to our database
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        string getSurveySQLString = "select Count(survey_result.parkCode)as survey_count," +
        //                                    " park.parkName, park.parkCode " +
        //                                    "from survey_result " +
        //                                    "join park on survey_result.parkCode = park.parkCode " +
        //                                    "group by parkName, park.parkCode order by survey_count desc, parkName asc";

        //        SqlCommand cmd = new SqlCommand(getSurveySQLString, connection);

        //        // Execute the query to the database
        //        SqlDataReader reader = cmd.ExecuteReader();

        //        // The results come back as a SqlDataReader. Loop through each of the rows
        //        // and add to the output list
        //        while (reader.Read())
        //        {
        //            SurveyResultsViewModel survey = new SurveyResultsViewModel();
        //            survey.ParkName = Convert.ToString(reader["parkName"]);
        //            survey.ParkCode = Convert.ToString(reader["parkCode"]);
        //            survey.NumberOfSurvey = Convert.ToInt32(reader["survey_count"]);
        //            output.Add(survey);
        //        }
        //    }
        //    return output;
        //}
        //#endregion
    }
}
