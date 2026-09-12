using Microsoft.Data.SqlClient;

namespace WebApplication
{
    public class PowerDataDAL
    {
        private List<PowerData> _ElencoPower = new List<PowerData>();

        public List<PowerData> GetAll()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DBTurbinStats;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
            {
                connection.Open();

                string sql = "SELECT Data, Power, id_power FROM Power_hourly";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        _ElencoPower.Clear();
                        while (reader.Read())
                        {
                            PowerData power = new PowerData();
                            power.Data = reader.GetDateTime(0);
                            power.ActivePower = reader.GetDouble(1);
                            power.id_power = reader.GetInt32(2);
                            _ElencoPower.Add(power);
                        }
                    }
                }
            }

            return _ElencoPower;
        }
    }
}
