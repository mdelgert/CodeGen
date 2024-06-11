using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CodeGen.WebDesigner.Startup))]
namespace CodeGen.WebDesigner
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
