using System;
using System.Diagnostics;

namespace Tools.Extensions
{
    public static class ExceptionExtensions
    {
        public static void SaveError(this Exception e)
        {
            SaveError(e, null);
        }

        public static void SaveError(this Exception e, Action<Exception> saveErrorLog)
        {
            if (e == null) 
                throw new ArgumentNullException("e");

            if (saveErrorLog != null)
                try
                {
                    saveErrorLog(e);
                }
                catch (Exception exc)
                {
                    Debug.WriteLine(exc);
                }
            else
            {
                Debug.WriteLine(e);
            }
        }

        /// <summary>
        /// Получает полное сообщение об ошибке со всех внутренних исключений
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetErrorMessage(this Exception e)
        {
            if (e == null)
                return string.Empty;

            return string.Format("{0}. Внутреннее сообщение: {1}", e.Message, GetErrorMessage(e.InnerException));
        }
    }
}
