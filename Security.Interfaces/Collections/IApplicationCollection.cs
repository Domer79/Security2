using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Security.Interfaces.Base;
using Security.Interfaces.Model;

namespace Security.Interfaces.Collections
{
    /// <summary>
    /// Интерфейс, представляющий коллекцию приложений
    /// </summary>
    public interface IApplicationCollection : IQueryableCollection<IApplication>
    {
    }
}
