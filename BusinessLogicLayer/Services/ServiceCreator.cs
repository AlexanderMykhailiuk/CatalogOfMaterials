using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Injections;
using Ninject;
using DataAccessLayer;

namespace BusinessLogicLayer.Services
{
    public class ServiceCreator : IServiceCreator
    {
        public IUserService CreateUserService(string connection)
        {
            var module = new BusinessLogicModule(connection);
            var kernel = new StandardKernel(module);
            return new UserService(kernel.Get<IUnitOfWork>());
        }
    }
}
