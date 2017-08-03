using System;

namespace Security.EntityFramework.Exceptions
{
    /// <summary>
    /// Происходит при поиске и отсутствии в базе данных определенного объекта безопасности
    /// </summary>
    public class SecurityObjectNotFoundException : Exception
    {
        public SecurityObjectNotFoundException(string objectName)
            :base($"Объект безопасности {objectName} не найден.")
        {
            
        }
    }
}