using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(bigschool.Startup))]
namespace bigschool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
