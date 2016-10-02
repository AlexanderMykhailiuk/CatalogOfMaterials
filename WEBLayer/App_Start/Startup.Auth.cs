using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.Interfaces;
using System.Web.Configuration;

[assembly: OwinStartup(typeof(WEBLayer.Startup))]

namespace WEBLayer
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {            
            app.CreatePerOwinContext<IUserService>(CreateUserService);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }

        IServiceCreator serviceCreator = new ServiceCreator();

        private IUserService CreateUserService()
        {
            string connection = WebConfigurationManager.ConnectionStrings["CatalogConnectionString"].ConnectionString;
            return serviceCreator.CreateUserService(connection);
        }
    }
}