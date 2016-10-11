using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Itis.ProxyDataPlatform.Attributes;

namespace Itis.ProxyDataPlatform.Base
{
    [MapTo(typeof(NamedEntityBll))]
    public class NamedEntityBll: EntityBll
    {
        public string Name { get; set; }
    }
}
