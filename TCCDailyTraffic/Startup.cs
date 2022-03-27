using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TCCDailyTraffic.Startup))]
namespace TCCDailyTraffic
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
