using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace Itis.ProxyDataPlatform.DatabaseContext
{
    public class ContainerConfiguration : DbConfiguration
    {
        /// <summary>
        /// Any class derived from <see cref="T:System.Data.Entity.DbConfiguration"/> must have a public parameterless constructor
        ///             and that constructor should call this constructor.
        /// </summary>
        protected internal ContainerConfiguration()
        {
            SetProviderServices(SqlProviderServices.ProviderInvariantName, SqlProviderServices.Instance);
        }
    }
}