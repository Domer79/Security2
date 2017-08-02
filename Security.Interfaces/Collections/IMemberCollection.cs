using System;
using System.Collections.Generic;
using System.Linq;
using Security.Interfaces.Model;
using Security.Interfaces.Base;

namespace Security.Interfaces.Collections
{
    /// <summary>
    /// Интерфейс, представляющий собой коллекцию участников безопасности
    /// </summary>
    public interface IMemberCollection : ISecurityQueryable<IMember>
    {
        /// <summary>
        /// Возвращает участников безопасности с информацией об их ролях
        /// </summary>
        /// <returns></returns>
        IQueryable<IMember> WithRoles();
    }
}