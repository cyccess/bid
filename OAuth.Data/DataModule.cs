using Autofac;
using OAuth.Core.Interfaces;
using OAuth.Data.Repositories;
using OAuth.Domain.IRepository;


namespace OAuth.Data
{
    public class DataModule : Module
    {
        /// <summary>
        /// 数据库链接字符串或者名称
        /// </summary>
        private readonly string _nameOrConnectionString;

        public DataModule(string nameOrConnectionString)
        {
            _nameOrConnectionString = nameOrConnectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new OAuthContext(_nameOrConnectionString)).As<IDbContext>().InstancePerRequest();
            builder.RegisterType<BaseRepository>().As<IRepository>().InstancePerRequest();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();

            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerRequest();
            builder.RegisterType<ReportRepository>().As<IReportRepository>().InstancePerRequest();

            base.Load(builder);
        }
    }
}
