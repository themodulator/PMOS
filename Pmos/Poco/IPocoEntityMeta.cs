using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmos.Poco
{
    public interface IPocoEntityMeta
    {
        string TypeFullName { get; set; }

        string TypeName { get; set; }

        string Colloquial { get; set; }

        string Pluralized { get; set; }

        string Table { get; set; }

        string Schema { get; set; }
    }
}
