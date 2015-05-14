using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CreativeWebDesign_dotNet_Project.Startup))]
namespace CreativeWebDesign_dotNet_Project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
