using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MedicineEshop.Startup))]
namespace MedicineEshop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
