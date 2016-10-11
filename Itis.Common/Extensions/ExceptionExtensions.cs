using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itis.Common.Extensions
{
    public static class ExceptionExtensions
    {
        public static void SaveError(this Exception e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            if (ApplicationCustomizer.SaveErrorLog != null)
                try
                {
                    ApplicationCustomizer.SaveErrorLog(e);
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
        /// Возвращает полную строку ошибки, собранную со всех вложенных <see cref="Exception.InnerException"/>
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetErrorMessage(this Exception ex)
        {
            if (ex == null)
                return string.Empty;

            return string.Format("{0}.\r\nInner exception: {1}", ex.Message, GetErrorMessage(ex.InnerException));
        }
    }
}
