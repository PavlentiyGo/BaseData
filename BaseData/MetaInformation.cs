using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseData
{
    internal class MetaInformation
    {
        public static string[] tables = new string[4];
        public static string[] columnsClients;
        public static string[] columnsGoods;
        public static string[] columnsOrders;
        public MetaInformation()
        {
            tables = GetTableNamesArray();
            columnsClients = GetColumnNames(AppSettings.SqlConnection, tables[0]);
            columnsGoods = GetColumnNames(AppSettings.SqlConnection, tables[1]);
            columnsOrders = GetColumnNames(AppSettings.SqlConnection, tables[3]);
        }

        public void OuputNames()
        {
            foreach (var table in tables)
            {
                MessageBox.Show(table);
            }
        }
        public static string[] GetTableNamesArray(string schemaName = "public")
        {
            string connectionString = AppSettings.SqlConnection;
            const string sql = @"
        SELECT table_name
        FROM information_schema.tables
        WHERE table_schema = @schema
          AND table_type = 'BASE TABLE'
        ORDER BY table_name;";

            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("schema", schemaName);

            var tables = new string[4];
            int index = 0;

            using var reader = cmd.ExecuteReader(); // ← синхронное выполнение
            while (reader.Read()) // ← синхронное чтение
            {
                if (index >= 4)
                    throw new InvalidOperationException($"В схеме '{schemaName}' больше 4 таблиц.");

                tables[index] = reader.GetString(0);
                index++;
            }

            if (index != 4)
                throw new InvalidOperationException($"Найдено {index} таблиц, ожидалось 4.");

            return tables; // ← обычный return string[4]
        }
        static string[] GetColumnNames(string connectionString, string table, string schema = "public")
        {
            var columns = new List<string>();

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"
            SELECT column_name
            FROM information_schema.columns
            WHERE table_schema = @schema
              AND table_name = @table
            ORDER BY ordinal_position;";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("schema", schema);
                    cmd.Parameters.AddWithValue("table", table);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            columns.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return columns.ToArray();
        }
        public static void RefreshData(bool flag = false)
        {
            if (flag)
            {
                tables = GetTableNamesArray();
            }
            columnsClients = GetColumnNames(AppSettings.SqlConnection, tables[0]);
            columnsGoods = GetColumnNames(AppSettings.SqlConnection, tables[1]);
            columnsOrders = GetColumnNames(AppSettings.SqlConnection, tables[3]);
        }

        public static string[] GetConstraintNames(string tableName, string schema = "public")
        {
            string connectionString = AppSettings.SqlConnection;
            var constraints = new List<string>();

            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            // Запрос к information_schema для получения имён ограничений
            string sql = @"
        SELECT constraint_name
        FROM information_schema.table_constraints
        WHERE table_schema = @schema
          AND table_name = @table;";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("schema", schema);
            cmd.Parameters.AddWithValue("table", tableName);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                constraints.Add(reader["constraint_name"].ToString());
            }

            return constraints.ToArray();
        }
    }
}