using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Applr.Startup))]
namespace Applr
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
