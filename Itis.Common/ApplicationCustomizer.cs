using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Itis.Common.Extensions;

namespace Itis.Common
{
    public class ApplicationCustomizer
    {
        public static Action<Exception> SaveErrorLog { get; set; }

        public static ConnectionStringSettings ConnectionStringCollection { get; set; }

        public static string GetConnectionStringByDbName(string name)
        {
            var connectionStrings = ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>();
            foreach (var connectionString in connectionStrings)
            {
                var builder = new SqlConnectionStringBuilder(connectionString.ConnectionString);
                if (string.Equals(builder.InitialCatalog, name, StringComparison.OrdinalIgnoreCase))
                {
                    builder.InitialCatalog = "";
                    return builder.ConnectionString;
                }
            }

            return null;

            //Тоже самое только в выражении LINQ
            //            return (from connectionString in connectionStrings
            //                    let builder = new SqlConnectionStringBuilder(connectionString.ConnectionString)
            //                    where builder.DataSource == Name
            //                    select connectionString.ConnectionString).FirstOrDefault();
        }

        public static bool DatabaseExists(string databaseName)
        {
            var connectionString = GetConnectionStringByDbName(databaseName);
            if (connectionString == null)
                throw new InvalidOperationException(string.Format("ConnectionString for Database \"{0}\" not initialized", databaseName));

            using (var connection = new SqlConnection(connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "select DB_ID('" + databaseName + "')";
                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader(CommandBehavior.SingleRow);
                    try
                    {
                        if (reader.Read())
                        {
                            return reader[0] != DBNull.Value;
                        }

                        return false;
                    }
                    catch
                    {
                        return false;
                    }
                    finally
                    {
                        reader.Close();
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static void WriteLogFile(Exception e)
        {
            WriteStringToLogFile(e.Message);
            WriteStringToLogFile(e.StackTrace);
        }

        public static void WriteStringToLogFile(string message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Log";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path += "\\" + DateTime.Now.ToString("dd.MM.yyyy") + ".txt";
            using (StreamWriter sw = (File.Exists(path)) ? File.AppendText(path) : File.CreateText(path))
            {
                sw.WriteLine(DateTime.Now);
                sw.WriteLine(message);
            }
        }
    }
}