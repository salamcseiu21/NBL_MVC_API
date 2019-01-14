using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NBL.Areas.Accounts.BLL;
using NBL.Areas.Accounts.BLL.Contracts;
using NBL.Areas.Accounts.DAL;
using NBL.Areas.Accounts.DAL.Contracts;
using NBL.Areas.Admin.BLL;
using NBL.Areas.Admin.BLL.Contracts;
using NBL.Areas.Admin.DAL;
using NBL.Areas.Admin.DAL.Contracts;
using NBL.Areas.Factory.BLL;
using NBL.BLL;
using NBL.BLL.Contracts;
using NBL.Controllers;
using NBL.DAL;
using NBL.DAL.Contracts;
using NBL.Models;
using System.Data.Entity;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Mvc5;

namespace NBL
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            container.RegisterType<IBranchGateway, BranchGateway>();
            container.RegisterType<IBranchManager, BranchManager>();
            container.RegisterType<IClientGateway, ClientGateway>();
            container.RegisterType<IClientManager, ClientManager>();
            container.RegisterType<ICommonGateway, CommonGateway>();
            container.RegisterType<ICommonManager, CommonManager>();
            container.RegisterType<ICompanyGateway, CompanyGateway>();
            container.RegisterType<ICompanyManager, CompanyManager>();
            container.RegisterType<IDepartmentManager, DepartmentManager>();
            container.RegisterType<IDepartmentGateway, DepartmentGateway>();
            container.RegisterType<IDesignationGateway, DesignationGateway>();
            container.RegisterType<IDesignationManager, DesignationManager>();
            container.RegisterType<IDiscountGateway, DiscountGateway>();
            container.RegisterType<IDiscountManager, DiscountManager>();

            container.RegisterType<IDistrictGateway, DistrictGateway>();
            container.RegisterType<IDistrictManager, DistrictManager>();
            container.RegisterType<IDivisionGateway, DivisionGateway>();

            container.RegisterType<IEmployeeGateway, EmployeeGateway>();
            container.RegisterType<IEmployeeManager, EmployeeManager>();
            container.RegisterType<IEmployeeTypeGateway, EmployeeTypeGateway>();
            container.RegisterType<IEmployeeTypeManager, EmployeeTypeManager>();

            container.RegisterType<IInventoryGateway, InventoryGateway>();
            container.RegisterType<IInventoryManager, InventoryManager>();

            container.RegisterType<IOrderGateway, OrderGateway>();
            container.RegisterType<IOrderManager, OrderManager>();

            container.RegisterType<IPostOfficeGateway, PostOfficeGateway>();
            container.RegisterType<IProductManager, ProductManager>();

            container.RegisterType<IProductGateway, ProductGateway>();
            container.RegisterType<IRegionGateway, RegionGateway>();
            container.RegisterType<IRegionManager, RegionManager>();

            container.RegisterType<IReportGateway, ReportGateway>();
            container.RegisterType<IReportManager, ReportManager>();
            container.RegisterType<ITerritoryGateway, TerritoryGateway>();
            container.RegisterType<ITerritoryManager, TerritoryManager>();
            container.RegisterType<IUpazillaGateway, UpazillaGateway>();
            container.RegisterType<IUserGateway, UserGateway>();
            container.RegisterType<IUserManager, UserManager>();

            container.RegisterType<IVatGateway, VatGateway>();
            container.RegisterType<IVatManager, VatManager>();
            container.RegisterType<IAccountsManager, AccountsManager>();
            container.RegisterType<IAccountGateway, AccountsGateway>();
            container.RegisterType<IInvoiceManager, InvoiceManager>();
            container.RegisterType<IInvoiceGateway, InvoiceGateway>();


            container.RegisterType<IDeliveryManager, Areas.Manager.BLL.DeliveryManager>();
            container.RegisterType<IDeliveryGateway, Areas.Manager.DAL.DeliveryGateway>();


            container.RegisterType<DbContext, ApplicationDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<UserManager<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<AccountController>(new InjectionConstructor());



        }
    }
}