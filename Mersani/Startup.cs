using Mersani.Controllers.Administrator;
using Mersani.Interfaces.Administrator;
using Mersani.Interfaces.Archive;
using Mersani.Interfaces.Auth;
using Mersani.Interfaces.CallCenter;
using Mersani.Interfaces.Finance;
using Mersani.Interfaces.FinancialSetup;
using Mersani.Interfaces.Notifications;
using Mersani.Interfaces.PointOfSale;
using Mersani.Interfaces.Purchase;
using Mersani.Interfaces.Sales;
using Mersani.Interfaces.Stock;
using Mersani.Interfaces.Users;
using Mersani.Interfaces.Website.CusAuth;
using Mersani.Interfaces.Website.Customer_;
using Mersani.Interfaces.Website.ItemGroups;
using Mersani.Interfaces.Website.items;
using Mersani.Interfaces.Website.WebShoping;
using Mersani.models.Hubs;
using Mersani.Repositories;
using Mersani.Repositories.Adminstrator;
using Mersani.Repositories.Archive;
using Mersani.Repositories.Auth;
using Mersani.Repositories.CallCenter;
using Mersani.Repositories.Finance;
using Mersani.Repositories.FinancialSetup;
using Mersani.Repositories.Notifications;
using Mersani.Repositories.PointOfSale;
using Mersani.Repositories.Purchase;
using Mersani.Repositories.Sales;
using Mersani.Repositories.Stock;
using Mersani.Repositories.Users;
using Mersani.Repositories.Website.Customer_;
using Mersani.Repositories.Website.CustRepo;
using Mersani.Repositories.Website.ItemGroups;
using Mersani.Repositories.Website.Items;
using Mersani.Repositories.Website.Shoping;
using Mersani.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.IO;
using Stripe;
using Mersani.models.website;
using Mersani.Interfaces.HR;
using Mersani.Repositories.HR;
namespace Mersani
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR(e => {
                e.MaximumReceiveMessageSize = 102400000;
                e.EnableDetailedErrors = true;
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins("http://localhost")
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyMethod();
                });
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            #region Auth
            services.AddTransient<IAuthRepository, AuthRepository>();
            services.AddTransient<IEmailService, EmailService>();

            services.AddTransient<IReportSettingsRepo, ReportSettingsRepository>();
            #endregion

            #region Notifications
            services.AddTransient<IReminderRepo, ReminderRepository>();
            #endregion

            #region Users
            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<IUserBranchActivityRepo, UserBranchActivityRepository>();
            services.AddTransient<IUserPrivilegeRepo, UserPrivilegeRepository>();
            services.AddTransient<IUserPharmaciesRepo, UserPharmaciesRepository>();
            #endregion

            #region Admininstrator
            services.AddTransient<ICurrenciesRepository, CurrenciesRepository>();
            services.AddTransient<IActivityRepo, ActivityRepository>();
            services.AddTransient<ICompanyRepo, CompanyRepository>();
            services.AddTransient<ICompanyBranchesRepo, CompanyBranchesRepository>();
            services.AddTransient<ICountryRepo, CountryRepository>();
            services.AddTransient<IRegionRepo, RegionRepository>();
            services.AddTransient<ICityRepo, CityRepository>();
            services.AddTransient<IMenuRepo, MenuRepository>();
            services.AddTransient<IUserGroupRepo, UserGroupRepository>();
            services.AddTransient<IGroupRepo, GroupRepository>();
            services.AddTransient<IBranchActivitiesRepo, BranchActivitiesRepository>();
            services.AddTransient<IUserGroupMenuRepo, UserGroupMenuRepository>();
            services.AddTransient<IGeneralSelectize, GeneralSelectizeRepository>();
            services.AddTransient<IUserTranslationRepo, UserTranslationsRepository>();
            services.AddTransient<IOwnerSetupRepo, OwnerSetupRepository>();
            services.AddTransient<IPharmacySetupRepo, PharmacySetupRepository>();
            services.AddTransient<IGeneralSetupRepo, GeneralsetupRepository>();
            services.AddTransient<TblArchivesRepo, TblArchivesRepository>();
            services.AddTransient<GeneralSharedRepo, GeneralSharedRepository>();
            services.AddTransient<IGeneralReportParmsRepo, GeneralReportParmsRepository>();
            services.AddTransient<IUserRolesRepo, UserRolesRepository>();
            services.AddTransient<IModulesSettingRepo, ModulesSettingRepository>();
            services.AddTransient<IPrinterSettingsRepo, PrinterSettingsRepository>();
            services.AddTransient<CountryLoyalityRepo, CountryLoyalityRepository>();
            services.AddTransient<CountryPromotionRepo, CountryPromotionRepository>();
            services.AddTransient<IWebAuthRepo, WebAuthRepository>();
            services.AddTransient<IWebMobileAdsRepo, WebMobileAdsRepository>();

            #endregion

            #region General Utilities
            services.AddSingleton<Database>();
            #endregion

            #region FinanicalSetup
            services.AddTransient<IFinisAccountRepository, FinsAccountRepository>();
            services.AddTransient<IFinsAccountClassRepository, FinsAccountClassReposatiory>();
            services.AddTransient<IFinsAccountLevelRepository, FinsAccountLevelRepository>();
            services.AddTransient<IFinsCostCeneterRepository, FinsCostCenterReposatiory>();
            services.AddTransient<ITNXSetupRepo, TNXSetupRepository>();
            services.AddTransient<ICustomerClassRepo, CustomerClassRepository>();
            services.AddTransient<ISupplierClassRepo, SupplierClassRepository>();
            services.AddTransient<IBankSetupRepo, BankSetupRepository>();
            services.AddTransient<IBudgetTypeRepo, BudgetTypeRepository>();
            services.AddTransient<ICustomerRepo, CustomerRepository>();
            services.AddTransient<ISupplierRepo, SupplierRepository>();
            services.AddTransient<IAccountSettingRepo, AccountSettingRepository>();
            services.AddTransient<IFixedAssetCategoriesRepo, FixedAssetCategoriesRepository>();
            services.AddTransient<IFixedAssetTypeRepo, FixedAssetTypeRepository>();
            services.AddTransient<IInsuranceCompanyRepo, InsuranceCompanyRepository>();
            services.AddTransient<IInsuranceContractRepo, InsuranceContractRepository>();
            #endregion

            #region Finanice
            services.AddTransient<IVoucherRepo, VoucherRepository>();
            services.AddTransient<IVoucherBudgetRepo, VoucherBudgetRepository>();
            services.AddTransient<IFinsFixedAssetRepo, FinsFixedAssetRepository>();
            services.AddTransient<IPeriodYearRepo, PeriodYearRepository>();
            services.AddTransient<OwnerSupplierRepo, OwnerSupplierRepository>();
            #endregion

            #region Sales
            services.AddTransient<ISalesInvoicesRepo, SalesInvoicesRepository>();
            services.AddTransient<ISalesPaymentRepo, SalesPaymentRepository>();
            services.AddTransient<SalesInvoicesReturnRepo, SalesInvoicesReturnRepository>();
            services.AddTransient<ISalesOrderRepo, SalesOrderRepository>();
            services.AddTransient<ISalesOrderReturnRepo, SalesOrderReturnRepository>();
            services.AddTransient<SalesDeleveryNoteRepo, SalesDeleveryNoteRepository>();
            services.AddTransient<SalesReturnDeleveryNoteRepo, SalesReturnDeleveryNoteRepository>();
            services.AddTransient<ISalesQuotationRepo, SalesQuotationRepository>();
            services.AddTransient<IPointOfSaleRepo, PointOfSaleRepository>();
            services.AddTransient<IPharmacyShiftRepo, PharmacyShiftRepository>();
            services.AddTransient<ICustomerPointsRepo, CustomerPointsRepository>();
            #endregion

            #region Purchase
            services.AddTransient<IPurchaseInvoicesRepo, PurchaseInvoicesRepository>();
            services.AddTransient<IPurchasePaymentRepo, PurchasePaymentRepository>();
            services.AddTransient<IPurchaseOrderRepo, PurchaseOrderRepository>();
            services.AddTransient<IPurchaseRequestRepo, PurchaseRequestRepository>();
            services.AddTransient<PurchaseInvoicesReturnRepo, PurchaseInvoicesReturnRepository>();
            services.AddTransient<InvPrchReturnOrdrRepo, InvPrchReturnOrdrRepository>();
            services.AddTransient<IPosRequestItemsRepo, PosRequestItemsRepository>();
            #endregion

            #region Stock
            services.AddTransient<IItemGroupsRepo, ItemGroupsRepository>();
            services.AddTransient<IItemsRepo, ItemsRepository>();
            services.AddTransient<IUnitsRepo, UnitsRepository>();
            services.AddTransient<IInventoryRepo, InventoryRepository>();
            services.AddTransient<IItemBatchesRepo, ItemBatchesRepository>();
            services.AddTransient<IInventoryTransferRepo, InventoryTransferRepository>();
            services.AddTransient<InvDepreciationRepo, InvDepreciationRepository>();
            services.AddTransient<ITransferRequestRepo, TransferRequestRepository>();
            services.AddTransient<IInventoryLocationsRepo, InventoryLocationsRepository>();
            services.AddTransient<IInventoryItemsRepo, InventoryItemsRepository>();
            services.AddTransient<InvAssmblyItemRepo, InvAssmblyItemRepository>();
            services.AddTransient<InvDeleveryNotesRepo, InvDeleveryNotesRepository>();
            services.AddTransient<InvRtrnDeleveryNotesRepo, InvRtrnDeleveryNotesRepository>();
            services.AddTransient<IItemDosageRepo, ItemDosageRepository>();
            services.AddTransient<IItemManufacturerRepo, ItemManufacturerRepository>();
            #endregion

            #region Archives
            services.AddTransient<GeneralArchiveSetupRepo, GeneralArchiveSetupRepository>();
            services.AddTransient<GeneralArchiveRepo, GeneralArchiveRepository>();

            #endregion

            #region CallCenter
            services.AddTransient<TktSalesOrderRepo, TktSalesOrderRepository>();
            services.AddTransient<ITicketMasterRepo, TicketMasterRepository>();
            services.AddTransient<ITktChatRepo, TktChatRepository>();

            #endregion

            #region website
            services.AddTransient<Iwebitems, WebItemsRepository>();
            services.AddTransient<IWebItemGroup, WebtemGroupRepository>();
            services.AddTransient<IWebShopingRepo, ShopingRepo>();
            services.AddTransient<IWebCustomer, WebCustomerRepo>();
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));

            #endregion
            #region HR
            services.AddTransient<HrDepartmentRepo, HrDepartmentRepository>();

            services.AddTransient<HrJobRepo, HrJobRepository>();
            services.AddTransient<HrBanksRepo, HrBanksRepository>();
            services.AddTransient<HrBonusesRepo, HrBonusesRepository>();
            services.AddTransient<CostCenterRepo, CostCentarRepository>();
            services.AddTransient<DeductionTypeRepo, HrDeductionRepository>();
            services.AddTransient<LoanTypeRepo, HrLoanTypeRepository>();
            services.AddTransient<HrNewHrDepartmentRepo, HrNewHrDepartmentRepository>();
            services.AddTransient<HrSitesRepo, HrSitesRepository>();




            services.AddTransient<HrQualificationRepo, HrQualificationRepository>();
            services.AddTransient<HrJobGroupsRepo, HrJobGroupsRepository>();
            services.AddTransient<HrJobTitlesRepo, HrJobTitlesRepository>();
            services.AddTransient<HrIncrementsTypesRepo, HrIncrementsTypesRepository>();
            services.AddTransient<HrExpensestypesRepo, HrExpensestypesRepository>();
            services.AddTransient<HrVacationRepo, HrVacationRepository>();
            #endregion


            services.AddSingleton<IConfiguration>(Configuration);
            services.AddScoped<MessageHubHelper>();

            services.AddCors();
            services.AddAuthentication(IISDefaults.AuthenticationScheme);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddJsonOptions(x => { });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "Mirsani Service API",
                    Version = "v2",
                    Description = "List of Valid APIs",
                });
            });

            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                .WithOrigins("http://localhost:4200", "http://localhost:7778", "http://192.168.1.10:4445", "http://localhost:4201")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            StripeConfiguration.ApiKey = "sk_test_51JQeO2LX1VYYECMdQpNlPr33pLNMHM21yTVS66gs0iukKgMSP6GUBMEk3yz6uNPnQ62pKjMxWeSMJkZu2EpySP4Q00Xv5sXXms";

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.Use(async (context, next) =>
                {
                    await next();
                    if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
                    {
                        context.Request.Path = "/index.html";
                        await next();
                    }
                });
            }

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), @"Resources"));
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });

            app.UseCors("CorsPolicy");
            app.UseCors(options =>
                        options.WithOrigins(
                           "http://localhost:4200/",
                           "http://localhost:7778/",
                           "http://192.168.1.10:4445/",
                           "http://{public IP}:{public port}/",
                           "http://{public DNS name}:{public port}/"
                        ).AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "Mirsani Services");
                options.DocExpansion(DocExpansion.None);
                options.DefaultModelsExpandDepth(-1);
            });

            app.UseRouting();

            app.UseAuthorization();

            //app.UseSignalR(routes =>
            //    routes.MapHub<NotificationsHub>("/NotificationsHub")
            //);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationsHub>("/NotificationsHub");
                endpoints.MapHub<MessageHub>("/MessageHub");
            });
        }
    }
}
