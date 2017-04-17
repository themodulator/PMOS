using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Pmos.Web
{
    public class MvcPath : IMvcPath
    {


        public MvcPath()
        { }

        public MvcPath(RouteData routeData)
        {

            this.Area = routeData.DataTokens["Area"] == null ? "" : routeData.DataTokens["Area"].ToString();

            this.Controller = routeData.Values["Controller"] == null ? "" : routeData.Values["Controller"].ToString();

            this.Action = routeData.Values["Action"] == null ? "" : routeData.Values["Action"].ToString();
            
            this.Id = routeData.DataTokens["Id"] == null ? "" : routeData.DataTokens["Id"].ToString();

        }

        public MvcPath(string area, string controller, string action)
        {

            this.Area = area;

            this.Controller = controller;

            this.Action = action;

        }

        public MvcPath(string area, string controller, string action, string id)
            :this(area, controller, action)
        {

            this.Id = id;

        }

        public MvcPath(string path)
        {
            if (!path.StartsWith("/"))
                path = "/" + path;

            string[] full = path.Split('?');

            if (full.Count() == 2)
                this.QueryString = full[1];

            string[] rec = full[0].Split('/');

            this.Area = (rec.Count() > 1) ? rec[1] : "";
            
            this.Controller = (rec.Count() > 2) ? rec[2] : "";

            this.Action = (rec.Count() > 3) ? rec[3] : "";
            
            if(rec.Count() == 5)
                this.Area = rec[4];
        }

        public string Id { get; set; }

        public string Area { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string QueryString { get; set; }

        public override string ToString()
        {
            string p = string.Format("/{0}/{1}/{2}", this.Area, this.Controller, this.Action);

            if (!string.IsNullOrEmpty(this.Id))
                p += string.Format("/{0}", this.Id);

            if (!string.IsNullOrEmpty(this.QueryString))
                p += string.Format("?{0}", this.QueryString);

            return p;
        }

    }
}
