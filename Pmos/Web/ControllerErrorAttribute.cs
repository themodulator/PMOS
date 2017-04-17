using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Pmos.Web
{
    public class ControllerErrorAttribute : HandleErrorAttribute
    {

        public ControllerErrorAttribute()
        { 
            
        }

        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
        }
        
    }
}
