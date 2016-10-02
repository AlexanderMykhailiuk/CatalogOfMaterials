using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;

namespace WEBLayer.Util
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IContentService>().To<ContentService>();
        }
    }
}