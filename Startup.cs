using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LocalTheatre.Web.Startup))]
namespace LocalTheatre.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
