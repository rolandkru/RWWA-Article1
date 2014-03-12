// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="">
//   
// </copyright>
// <summary>
//   The startup.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RWWA_Article1.Server
{
    using System.Web.Http;

    using Owin;

    /// <summary>
    /// The startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// This code configures Web API.
        /// </summary>
        /// <param name="appBuilder">
        /// The app builder.
        /// </param>
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for Self-Host
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(name: "DefaultApi", routeTemplate: "api/{controller}/{id}", defaults: new { id = RouteParameter.Optional });

            appBuilder.UseWebApi(config);
        }
    }
}