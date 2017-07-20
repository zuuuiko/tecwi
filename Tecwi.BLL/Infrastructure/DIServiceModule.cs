using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tecwi.DAL.Interfaces;
using Tecwi.DAL.Repositories;

namespace Tecwi.BLL.Infrastructure
{
    public class DIServiceModule : NinjectModule
    {
        private string connectionString;
        public DIServiceModule(string connection)
        {
            connectionString = connection;
        }
        public override void Load()
        {
            Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument(connectionString);
        }
    }
}
