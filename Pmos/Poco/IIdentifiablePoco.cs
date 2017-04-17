using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmos.Poco
{
    public interface IIdentifiablePoco
    {
        string Id { get; set; }
    }
}
