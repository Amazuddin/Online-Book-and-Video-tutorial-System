using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(onlineBookAndVideoTutorial.Startup))]
namespace onlineBookAndVideoTutorial
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
