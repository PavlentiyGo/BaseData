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
        public MetaInformation()
        {
            tables = GetTableNamesArray();
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
        conn.Open(); // ← синхронное открытие

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
}
}
