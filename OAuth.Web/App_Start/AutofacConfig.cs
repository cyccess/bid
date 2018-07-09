using Autofac;
using Autofac.Integration.Mvc;
using OAuth.Data;
using OAuth.Service;
using OAuth.Service.Common;
using OAuth.Service.Interfaces;
using OAuth.Web.Models;
using System.Configuration;
using System.Web.Mvc;

namespace OAuth.Web
{
    public class AutofacConfig : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }

        public static void ConfigureContainer()
        {
            //Autofac初始化
            var builder = new ContainerBuilder();

            // Register dependencies in controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register dependencies in filter attributes
            builder.RegisterFilterProvider();

            // Register dependencies in custom views
            builder.RegisterSource(new ViewRegistrationSource());

            // Register our Data dependencies
            //builder.RegisterModule(new DataModule(ConnectionStringEncrypt()));
            builder.RegisterModule(new DataModule("OAuthConnectionString"));


            builder.RegisterModule(new ServiceModule());

            builder.RegisterType<ProjectInfo>();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        /// <summary>
        /// 数据库连接字符串解密 
        /// </summary>
        /// <returns></returns>
        private static string ConnectionStringEncrypt()
        {
            string connString = ConfigurationManager.ConnectionStrings["OAuthConnectionString"].ConnectionString;
            string strEncryptConn = "";
            AESEDSVcrypts.AES_Decrypt(connString, out strEncryptConn);
            return strEncryptConn;
        }

        #region

        /*
        private static void SetupResolveRules(ContainerBuilder builder)
        {
            builder.RegisterControllers(typeof(MvcApplication).Assembly);//注册所有的Controller

            // Register dependencies in filter attributes
            builder.RegisterFilterProvider();

            // Register dependencies in custom views
            builder.RegisterSource(new ViewRegistrationSource());

            

            //UI项目只用引用OAuth.Service
            //如需加载实现的程序集
            var Services = Assembly.Load("OAuth.Service");
            var Repository = Assembly.Load("OAuth.Data");


            //根据名称约定（服务层的接口和实现均以Service结尾），实现服务接口和服务实现的依赖
            builder.RegisterAssemblyTypes(Services)
              .Where(t => t.Name.EndsWith("Service"))
              .AsImplementedInterfaces();


            //根据名称约定（数据访问层的接口和实现均以Repository结尾），实现数据访问接口和数据访问实现的依赖
            builder.RegisterAssemblyTypes(Repository)
              .Where(t => t.Name.EndsWith("Repository"))
              .AsImplementedInterfaces();



        }


       
        private void SetupResolveRules(ContainerBuilder builder)
        {
            builder.RegisterControllers(typeof(MvcApplication).Assembly);//注册所有的Controller

            #region 自动注册

            var assemblys = AppDomain.CurrentDomain.GetAssemblies();

            //builder.RegisterControllers(assemblys); //注册所有


            var baseType = typeof(IDependency);
            //所有服务Assembly
            var AllServices = assemblys
                .SelectMany(s => s.GetTypes())
                .Where(p => baseType.IsAssignableFrom(p) && p != baseType).Select(a => a.Assembly).ToArray();

            builder.RegisterControllers(AllServices);

            builder.RegisterAssemblyTypes(assemblys.ToArray())
                   .Where(t => baseType.IsAssignableFrom(t) && t != baseType)
                   .AsImplementedInterfaces().InstancePerLifetimeScope();

            #endregion

            #region 自定义注册服务

            //builder.RegisterType<UserService>().As<IUserService>();
            //builder.RegisterType<ModuleService>().As<IModuleService>();
            //builder.RegisterType<PermissionService>().As<IPermissionService>();
            //builder.RegisterType<ProjectService>().As<IProjectService>();
            //builder.RegisterType<RoleService>().As<IRoleService>();


            //builder.Register(o => new OAuthContext()).InstancePerHttpRequest();
            //builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>));

            #endregion

        }
        */

        #endregion
    }
}