using Autofac;
using OAuth.Service.Interfaces;

namespace OAuth.Service.Common
{
    /// <summary>
    /// Autofac 注册业务服务
    /// </summary>
    public class ServiceModule : Autofac.Module
    { 
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CacheManager>().As<ICacheManager>();

            builder.RegisterType<UserService>().As<IUserService>().InstancePerRequest();
            builder.RegisterType<ModuleService>().As<IModuleService>().InstancePerRequest();
            builder.RegisterType<PermissionService>().As<IPermissionService>().InstancePerRequest();
            builder.RegisterType<ProjectService>().As<IProjectService>().InstancePerRequest();
            builder.RegisterType<RoleService>().As<IRoleService>().InstancePerRequest();

            builder.RegisterType<ItemService>().As<IItemService>().InstancePerRequest();
            builder.RegisterType<ItemSureService>().As<IItemSureService>().InstancePerRequest();
            builder.RegisterType<ItemMaterialService>().As<IItemMaterialService>().InstancePerRequest();
            builder.RegisterType<ItemModeService>().As<IItemModeService>().InstancePerRequest();
            builder.RegisterType<DictService>().As<IDictService>().InstancePerRequest();
            builder.RegisterType<ModeService>().As<IModeService>().InstancePerRequest();
            builder.RegisterType<SupplierService>().As<ISupplierService>().InstancePerRequest();
            builder.RegisterType<AccountService>().As<IAccountService>().InstancePerRequest();
            builder.RegisterType<PurchaserService>().As<IPurchaserService>().InstancePerRequest();


            //UI项目只用引用OAuth.Service
            //如需加载实现的程序集
            //var Services = Assembly.Load("OAuth.Service");

            ////根据名称约定（服务层的接口和实现均以Service结尾），实现服务接口和服务实现的依赖
            //builder.RegisterAssemblyTypes(Services)
            //  .Where(t => t.Name.EndsWith("Service"))
            //  .AsImplementedInterfaces();

            base.Load(builder);
        }
    }
}
