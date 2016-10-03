using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Security.Model.Entities;

namespace Security.Model.EntityConfigurations
{
    public class LogConfiguration : BaseConfiguration<Log>
    {
        public LogConfiguration()
        {
            Property(e => e.Message).IsUnicode(false);
        }
    }
}
