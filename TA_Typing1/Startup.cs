using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TA_Typing1.Startup))]
namespace TA_Typing1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
