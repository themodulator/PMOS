using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmos.Web
{
    public interface IMvcPath
    {
        string Area { get; set; }

        string Controller { get; set; }

        string Action { get; set; }
    }
}
