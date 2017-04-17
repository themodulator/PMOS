using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Pmos;

namespace Pmos.WebApp
{
    public static class ApplicationExtensions
    {
        public static void FromApplication<TApp>(this TApp app, Assembly assembly)
            where TApp : IApplicationInfo
        {
            
            app.Title = assembly.Title();
            app.Copyright = assembly.Copyright();
            app.Description = assembly.GetAssemblyAttribute<AssemblyDescriptionAttribute>("Description");
            app.Trademark = assembly.GetAssemblyAttribute<AssemblyTrademarkAttribute>("Trademark");
            app.Company = assembly.Company();

        }


    }
}
