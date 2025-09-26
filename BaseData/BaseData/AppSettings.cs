using Npgsql;
using System.Data;

namespace BaseData
{
    public static class AppSettings
    {
        public static string? SqlConnection { get; set; }

        public static bool IsConnectionStringSet => !string.IsNullOrEmpty(SqlConnection);

        public static bool TestConnection()
        {
            try
            {
                if (!IsConnectionStringSet) return false;

                using (var connection = new NpgsqlConnection(SqlConnection))
                {
                    connection.Open();
                    return connection.State == ConnectionState.Open;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}