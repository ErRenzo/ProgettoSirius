using Microsoft.Data.SqlClient;

namespace WebApplication
{
    public class WindDataDAL
    {
        private List<WindData> _ElencoWind = new List<WindData>();

        public List<WindData> GetAll()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DBTurbinStats;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
            {
                connection.Open();

                string sql = "SELECT Data, Windspeed, id_wind FROM wind_cleaned";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        _ElencoWind.Clear();
                        while (reader.Read())
                        {
                            WindData wind = new WindData();
                            wind.Data = reader.GetDateTime(0);
                            wind.windSpeed = reader.GetDouble(1);
                            //Modifico la velocità del vento in m/s
                            wind.windSpeed = wind.windSpeed / 3.6;
                            wind.id_wind = reader.GetInt32(2);
                            _ElencoWind.Add(wind);
                        }
                    }
                }
            }

            return _ElencoWind;
        }
    }
}