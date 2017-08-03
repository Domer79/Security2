using System;
using Security.Interfaces.Model;

namespace Security.EntityFramework.Exceptions
{
    /// <summary>
    /// Происходит при поиске и отсутствии в базе данных определенного разрешения
    /// </summary>
    public class GrantNotFoundException : Exception
    {
        public GrantNotFoundException(int idSecObject, int idRole, int idAccessType)
            :base($"Разрешение {idSecObject}, {idRole}, {idAccessType} не найдено.")
        {
        }
    }
}