using Ninject.Modules;
using DataAccessLayer;

namespace BusinessLogicLayer.Injections
{
    public class BusinessLogicModule : NinjectModule
    {
        private string connection;
        public BusinessLogicModule (string connection)
        {
            this.connection = connection;
        }
        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument(connection);
        }
    }
}
