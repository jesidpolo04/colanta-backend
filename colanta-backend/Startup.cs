using colanta_backend.App.Users.Domain;
using colanta_backend.App.Users.Infraestructure;
using colanta_backend.App.Categories.Jobs;

using colanta_backend.App.Brands.Domain;
using colanta_backend.App.Brands.Infraestructure;
using colanta_backend.App.Brands.Jobs;

using colanta_backend.App.Shared.Application;
using colanta_backend.App.Shared.Domain;
using colanta_backend.App.Shared.Infraestructure;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace colanta_backend
{
    using App.Brands.Jobs;
    using App.Categories.Jobs;
    using App.Products.Jobs;
    using App.Prices.Jobs;
    using App.Inventory.Jobs;
    using App.Promotions.Jobs;
    using App.Orders.Jobs;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("Ecommerce", policy =>
                {
                    policy.WithOrigins("https://colanta.myvtex.com", "https://www.pidecolanta.com")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            //Dependencies Injections Users
            services.AddTransient<UsersRepository, UsersEFRepository>();
            services.AddTransient<App.Users.Domain.UsersSiesaRepository, App.Users.Infraestructure.UsersSiesaRepository>();
            services.AddTransient<App.Users.Domain.UsersVtexRepository, App.Users.Infraestructure.UsersVtexRepository>();
            services.AddTransient<App.Users.Domain.RegisterUserService>();
            
            //Dependencies Injections Brands
            services.AddTransient<BrandsRepository, EFBrandsRepository>();
            services.AddTransient<BrandsVtexRepository, VtexBrandsRepository>();
            services.AddTransient<IRenderBrandsMail, RenderBrandsMail>();
            services.AddTransient<App.Brands.Jobs.RenderBrands>();
            //Dependencies Injections Cattegories
            services.AddTransient<App.Categories.Domain.CategoriesRepository , App.Categories.Infraestructure.CategoriesEFRepository>(); //s
            services.AddTransient<App.Categories.Domain.CategoriesVtexRepository, App.Categories.Infraestructure.CategoriesVtexRepository>();
            services.AddTransient<App.Categories.Domain.CategoriesSiesaRepository, App.Categories.Infraestructure.CategoriesMockSiesaRepository>();//
            services.AddTransient<App.Categories.Domain.IRenderCategoriesMail, App.Categories.Infraestructure.RenderCategoriesMail>();
            services.AddTransient<RenderCategories>();
            services.AddTransient<ActivateAllCategories>();
            services.AddTransient<MapFamiliesToStore>();
            //Dependencies Injections Products
            services.AddTransient<App.Products.Domain.ProductsRepository , App.Products.Infraestructure.ProductsEFRepository>();
            services.AddTransient<App.Products.Domain.ProductsVtexRepository, App.Products.Infraestructure.ProductsVtexRepository>();
            services.AddTransient<App.Products.Domain.SkusRepository, App.Products.Infraestructure.SkusEFRepository>();
            services.AddTransient<App.Products.Domain.SkusVtexRepository, App.Products.Infraestructure.SkusVtexRepository>();
            services.AddTransient<App.Products.Domain.ProductsSiesaRepository, App.Products.Infraestructure.ProductsSiesaRepository>();
            services.AddTransient<App.Products.Domain.IRenderProductsMail, App.Products.Infraestructure.RenderProductsMail>();
            services.AddTransient<App.Products.Domain.IInvalidBrandMail, App.Products.Infraestructure.InvalidBrandMail>();
            services.AddTransient<App.Products.Domain.IInvalidCategoryMail, App.Products.Infraestructure.InvalidCategoryMail>();
            services.AddTransient<App.Products.Domain.GetSkuVtexIdBySiesaId>();
            
            services.AddTransient<App.Products.Jobs.RenderProductsAndSkus>();
            services.AddTransient<App.Products.Jobs.UpToVtexNullProductsAndSkus>();
            services.AddTransient<App.Products.Jobs.FixProductSkus>();
            //Dependencies Injections Specifications
            services.AddTransient<App.Specifications.Domain.SpecificationsVtexRepository, App.Specifications.Infraestructure.SpecificationsVtexRepository>();
            //Dependencies Injections Prices
            services.AddTransient<App.Prices.Domain.PricesRepository , App.Prices.Infraestructure.PricesEFRepository>();
            services.AddTransient<App.Prices.Domain.PricesVtexRepository, App.Prices.Infraestructure.PricesVtexRepository>();
            services.AddTransient<App.Prices.Domain.PricesSiesaRepository, App.Prices.Infraestructure.PricesSiesaRepository>();
            services.AddTransient<App.Prices.Domain.IRenderPricesMail, App.Prices.Infraestructure.RenderPricesMail>();
            services.AddTransient<App.Prices.Domain.INotifyMissingPriceMail, App.Prices.Infraestructure.NotifyMissingPricesMail>();
            services.AddTransient<App.Prices.Jobs.RenderPrices>();
            services.AddTransient<App.Prices.Jobs.NotifyMissingPrices>();
            //Depndencies Injectios Inventory
            services.AddTransient<App.Inventory.Domain.InventoriesRepository, App.Inventory.Infraestructure.InventoriesEFRepository>();
            services.AddTransient<App.Inventory.Domain.InventoriesVtexRepository, App.Inventory.Infraestructure.InventoriesVtexRepository>();
            services.AddTransient<App.Inventory.Domain.InventoriesSiesaRepository, App.Inventory.Infraestructure.InventoriesSiesaRepository>();
            services.AddTransient<App.Inventory.Domain.WarehousesRepository, App.Inventory.Infraestructure.WarehousesEFRepository>();
            services.AddTransient<App.Inventory.Domain.WarehousesSiesaVtexRepository, App.Inventory.Infraestructure.WarehousesSiesaVtexRepository>();
            services.AddTransient<App.Inventory.Domain.IRenderInventoriesMail, App.Inventory.Infraestructure.RenderInventoriesMail>();
            services.AddTransient<App.Inventory.Jobs.RenderInventories>();
            //Dependencies Injections Promotions
            services.AddTransient<App.Promotions.Domain.PromotionsRepository, App.Promotions.Infraestructure.PromotionsEFRepository>();
            services.AddTransient<App.Promotions.Domain.PromotionsVtexRepository, App.Promotions.Infraestructure.PromotionsVtexRepository>();
            services.AddTransient<App.Promotions.Domain.PromotionsSiesaRepository, App.Promotions.Infraestructure.PromotionsSiesaRepository>();
            services.AddTransient<App.Promotions.Jobs.RenderPromotions>();
            services.AddTransient<App.Promotions.Jobs.UpdatePromotionsState>();
            services.AddTransient<App.Promotions.Domain.IInvalidPromotionMail, App.Promotions.Infraestructure.InvalidPromotionMail>();
            services.AddTransient<App.Promotions.Domain.IRenderPromotionsMail, App.Promotions.Infraestructure.RenderPromotionsMail>();
            //Dependencies Injections GiftCards
            services.AddTransient<App.GiftCards.Domain.GiftCardsRepository, App.GiftCards.Infraestructure.GiftCardsEFRepository>();
            services.AddTransient<App.GiftCards.Domain.GiftCardsSiesaRepository, App.GiftCards.Infraestructure.GiftCardsSiesaRepository>();
            //Dependencies Injections Customer Credit
            services.AddTransient<App.CustomerCredit.Domain.CreditAccountsRepository, App.CustomerCredit.Infraestructure.CreditAccountsEFRepository>();
            services.AddTransient<App.CustomerCredit.Domain.CreditAccountsVtexRepository, App.CustomerCredit.Infraestructure.CreditAccountsVtexRepository>();
            services.AddTransient<App.CustomerCredit.Domain.CreditAccountsSiesaRepository, App.CustomerCredit.Infraestructure.CreditAccountsSiesaRepository>();
            services.AddTransient<App.CustomerCredit.Domain.IRenderAccountsMail, App.CustomerCredit.Infraestructure.RenderAccountsMail>();
            services.AddTransient<App.CustomerCredit.Domain.IInvalidAccountsMail, App.CustomerCredit.Infraestructure.InvalidAccountsMail>();
            services.AddTransient<App.CustomerCredit.Jobs.ReduceVtexCreditLimit>();
            services.AddTransient<App.CustomerCredit.Jobs.UpdateAccountsBalance>();
            services.AddTransient<App.CustomerCredit.Jobs.RenderCreditAccounts>();
            services.AddTransient<App.CustomerCredit.Domain.IRenderAccountsMail, App.CustomerCredit.Infraestructure.RenderAccountsMail>();
            //Dependencies Injections Credits
            services.AddTransient<App.Credits.Domain.CupoLacteoSiesaRepository, App.Credits.Infraestructure.CupoLacteoSiesaRepository>();
            //Dependencies Injections Orders
            services.AddTransient<App.Orders.Domain.OrdersRepository, App.Orders.Infraestructure.OrdersEFRepository>();
            services.AddTransient<App.Orders.Domain.OrdersVtexRepository, App.Orders.Infraestructure.OrdersVtexRepository>();
            services.AddTransient<App.Orders.Domain.OrdersSiesaRepository, App.Orders.Infraestructure.OrdersSiesaRepository>();
            services.AddTransient<App.Orders.Domain.FailOrderMailLogsRepository, App.Orders.Infraestructure.FailOrderMailLogsEFRepository>();
            services.AddTransient<App.Orders.Domain.INewOrderMail, App.Orders.Infraestructure.NewOrderMail>();
            services.AddTransient<App.Orders.Domain.ISiesaErrorAtSendOrderMail, App.Orders.Infraestructure.SiesaErrorAtSendOrderMail>();
            services.AddTransient<App.Orders.Domain.MailService>();
            //Dependencies Injections SiesaOrders
            services.AddTransient<App.Orders.SiesaOrders.Domain.SiesaOrdersRepository, App.Orders.SiesaOrders.Infraestructure.SiesaOrdersEFRepository>();
            services.AddTransient<App.Orders.SiesaOrders.Domain.SiesaOrdersHistoryRepository, App.Orders.SiesaOrders.Infraestructure.SiesaOrdersHistoryEFRepository>();
            services.AddTransient<App.Orders.Jobs.UpdateSiesaOrders, App.Orders.Jobs.UpdateSiesaOrders>();
            services.AddTransient<App.Orders.Domain.GetOrderDetailsVtexId>();

            //Dependencies Injections Shared
            services.AddTransient<WrongAddressesRepository, WrongAddressesEFReppository>();
            services.AddTransient<IProcess, ProcessLogs>();
            services.AddTransient<ILogger, EFLogger>();
            services.AddTransient<EmailSender, ColantaSender>();

            //------------------- Tasks -------------------------//

            //Scheduled Tasks
            services.AddHostedService<ScheduledRenderBrands>();
            services.AddHostedService<ScheduledUpdateBrandsState>();
            services.AddHostedService<ScheduledUpBrandsToVtex>();

            services.AddHostedService<ScheduledRenderCategories>();
            services.AddHostedService<ScheduledUpCategoriesToVtex>();
            services.AddHostedService<ScheduledUpdateCategoriesState>();

            services.AddHostedService<ScheduledRenderProductsAndSkus>();
            services.AddHostedService<ScheduledUpToVtexNullProductsAndSkus>();
            services.AddHostedService<ScheduledUpdateProductsAndSkusStates>();

            services.AddHostedService<ScheduledRenderPrices>();
            services.AddHostedService<ScheduledNotifyMissingPrices>();

            services.AddHostedService<ScheduledRenderInventories>();

            services.AddHostedService<ScheduledRenderPromotions>();
            services.AddHostedService<ScheduledUpdatePromotionsState>();
            services.AddHostedService<ScheduledUpToVtexNullPromotions>();

            services.AddHostedService<ScheduledUpdateSiesaOrders>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "colanta_backend", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "colanta_backend v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
