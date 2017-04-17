using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmos.WebApp
{
    public interface IApplicationInfo
    {

        string Title { get; set; }

        string Guid { get; set; }

        string Copyright { get; set; }

        string Description { get; set; }

        string Trademark { get; set; }

        string Company { get; set; }

    }
}
