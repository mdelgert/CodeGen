using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using CodeGen.Core.Data;

namespace CodeGen.WebDesigner
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //http://aspboss.blogspot.com/2012/03/model-backing-mydbcontext-context-has.html
            Database.SetInitializer<CodeGenContext>(new DropCreateDatabaseIfModelChanges<CodeGenContext>());
            //Database.SetInitializer<CodeGenContext>(new DropCreateDatabaseAlways<CodeGenContext>());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
