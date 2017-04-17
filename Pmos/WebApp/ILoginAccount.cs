using Pmos.Poco;
using Pmos.Poco.Humanities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmso.WebApp
{
    public interface ILoginAccount : IPersonName, IRecordAuthorship
    {

        LoginType LoginType { get; set; }

        string DisplayName { get; set; }

        string Password { get; set; }

        string NewPasword { get; set; }

        string ConfirmPassword { get; set; }

        DateTime PasswordDate { get; set; }

    }
}
