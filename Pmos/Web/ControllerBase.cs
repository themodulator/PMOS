
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Pmos.Web
{
    [ControllerError(View = "_error_partial")]
    public abstract class ControllerBase<TDbContext> : Controller
        where TDbContext : DbContext, new()
    {

        public ControllerBase()
        {
            this.DbContext = new TDbContext();

            
        }

        public TDbContext DbContext { get; private set; }

        public ActionResult HandleContextError()
        {
            return View("_error_partial");
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
        }
    }
}
