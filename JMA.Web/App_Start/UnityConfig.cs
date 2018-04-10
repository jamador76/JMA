using System.Web.Mvc;
using JMA.BusinessLogic.Services;
using Unity;
using Unity.Mvc5;

namespace JMA.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IClaimService, ClaimService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}