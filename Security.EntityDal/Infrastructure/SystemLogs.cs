using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Security.EntityDal.Infrastructure
{
    /// <summary>
    /// Класс, представляющий собой механизм логирования работы контекста БД
    /// </summary>
    public class SystemLogs
    {
        public static async void SaveLogTaskAsync(string log)
        {
//            await TaskToSaveLog(log);
            await Task.Run(() => SaveLog(log));
        }

//        [DebuggerStepThrough]
        public static void SaveLog(string log)
        {
            if (log == Environment.NewLine)
                return;

            try
            {
                using (var connection = new SqlConnection(Tools.ConnectionString))
                {
                    var command = new SqlCommand("Insert into sec.Log(message) values(@log)", connection);
                    var p = command.Parameters.Add("log", SqlDbType.NVarChar);
                    p.Value = log;

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static Task TaskToSaveLog(string log)
        {
            return new Task(() => SaveLog(log));
        }
    }
}