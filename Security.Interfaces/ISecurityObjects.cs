using System.Collections.Generic;

namespace Security.Interfaces
{
    /// <summary>
    /// Интерфейс, представляющий коллекцию объектов безопасности
    /// </summary>
    public interface ISecurityObjects : IEnumerable<ISecurityObject>
    {
    }
}