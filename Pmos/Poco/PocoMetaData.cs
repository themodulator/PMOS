using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmos.Poco
{
    public class PocoMetaData : IPocoEntityMeta
    {

        public string TypeFullName { get; set; }

        public string TypeName { get; set; }

        public string Colloquial { get; set; }

        public string Pluralized { get; set; }

        public string Table { get; set; }

        public string Schema { get; set; }

    }
}
