using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmos.Poco.Humanities
{
    public interface IPersonName
    {
        string Surname { get; set; }

        string GivenName { get; set; }

        string MiddleName { get; set; }

        string Generation { get; set; }

        string Prefix { get; set; }

        string Suffix { get; set; }

    }
}
