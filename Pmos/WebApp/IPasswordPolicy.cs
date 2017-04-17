using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pmos.WebApp
{
    public interface IPasswordPolicy
    {
        int RequiredLength { get; set; }

        bool RequireDigit { get; set; }

        bool RequireLowercase { get; set; }

        bool RequireNonLetterOrDigit { get; set; }

        bool RequireUppercase { get; set; }

        int Lifetime { get; set; }
    }
}
