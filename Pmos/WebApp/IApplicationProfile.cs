using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmos.WebApp
{
    public interface IApplicationProfile
    {
        string Theme { get; set; }

        string DisplayName { get; set; }

    }
}
