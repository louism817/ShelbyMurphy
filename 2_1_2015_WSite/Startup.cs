using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_2_1_2015_WSite.Startup))]
namespace _2_1_2015_WSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
