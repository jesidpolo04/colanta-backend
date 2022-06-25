using colanta_backend.App.Users.Domain;
using colanta_backend.App.Users.Infraestructure;
using colanta_backend.App.Brands.Jobs;
using colanta_backend.App.Categories.Jobs;
using colanta_backend.App.Products.Jobs;
using colanta_backend.App.Prices.Jobs;
using colanta_backend.App.Brands.Domain;
using colanta_backend.App.Brands.Infraestructure;
using colanta_backend.App.Shared.Application;
using colanta_backend.App.Shared.Domain;
using colanta_backend.App.Inventory.Jobs;
using colanta_backend.App.Promotions.Jobs;
using colanta_backend.App.CustomerCredit.Jobs;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using colanta_backend.App.Shared.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace colanta_backend
{
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
            //Dependencies Injections Brands
            services.AddTransient<UsersRepository, UsersEFRepository>();
            services.AddTransient<App.Users.Domain.UsersSiesaRepository, App.Users.Infraestructure.UsersSiesaRepository>();
            //Dependencies Injections Brands
            services.AddTransient<BrandsRepository, EFBrandsRepository>();
            services.AddTransient<BrandsVtexRepository, VtexBrandsRepository>();
            //Dependencies Injections Cattegories
            services.AddTransient<App.Categories.Domain.CategoriesRepository , App.Categories.Infraestructure.CategoriesEFRepository>(); //s
            services.AddTransient<App.Categories.Domain.CategoriesVtexRepository, App.Categories.Infraestructure.CategoriesVtexRepository>();
            services.AddTransient<App.Categories.Domain.CategoriesSiesaRepository, App.Categories.Infraestructure.CategoriesMockSiesaRepository>();//s
            //Dependencies Injections Products
            services.AddTransient<App.Products.Domain.ProductsRepository , App.Products.Infraestructure.ProductsEFRepository>();
            services.AddTransient<App.Products.Domain.ProductsVtexRepository, App.Products.Infraestructure.ProductsVtexRepository>();
            services.AddTransient<App.Products.Domain.SkusRepository, App.Products.Infraestructure.SkusEFRepository>();
            services.AddTransient<App.Products.Domain.SkusVtexRepository, App.Products.Infraestructure.SkusVtexRepository>();
            services.AddTransient<App.Products.Domain.ProductsSiesaRepository, App.Products.Infraestructure.ProductsSiesaRepository>();
            //Dependencies Injections Prices
            services.AddTransient<App.Prices.Domain.PricesRepository , App.Prices.Infraestructure.PricesEFRepository>();
            services.AddTransient<App.Prices.Domain.PricesVtexRepository, App.Prices.Infraestructure.PricesVtexRepository>();
            services.AddTransient<App.Prices.Domain.PricesSiesaRepository, App.Prices.Infraestructure.PricesSiesaRepository>();
            //Depndencies Injectios Inventory
            services.AddTransient<App.Inventory.Domain.InventoriesRepository, App.Inventory.Infraestructure.InventoriesEFRepository>();
            services.AddTransient<App.Inventory.Domain.InventoriesVtexRepository, App.Inventory.Infraestructure.InventoriesVtexRepository>();
            services.AddTransient<App.Inventory.Domain.InventoriesSiesaRepository, App.Inventory.Infraestructure.InventoriesSiesaRepository>();
            services.AddTransient<App.Inventory.Domain.WarehousesRepository, App.Inventory.Infraestructure.WarehousesEFRepository>();
            services.AddTransient<App.Inventory.Domain.WarehousesSiesaVtexRepository, App.Inventory.Infraestructure.WarehousesSiesaVtexRepository>();
            //Dependencies Injections Promotions
            services.AddTransient<App.Promotions.Domain.PromotionsRepository, App.Promotions.Infraestructure.PromotionsEFRepository>();
            services.AddTransient<App.Promotions.Domain.PromotionsVtexRepository, App.Promotions.Infraestructure.PromotionsVtexRepository>();
            services.AddTransient<App.Promotions.Domain.PromotionsSiesaRepository, App.Promotions.Infraestructure.PromotionsSiesaRepository>();
            //Dependencies Injections GiftCards
            services.AddTransient<App.GiftCards.Domain.GiftCardsRepository, App.GiftCards.Infraestructure.GiftCardsEFRepository>();
            services.AddTransient<App.GiftCards.Domain.GiftCardsSiesaRepository, App.GiftCards.Infraestructure.GiftCardsSiesaRepository>();
            //Dependencies Injections Customer Credit
            services.AddTransient<App.CustomerCredit.Domain.CreditAccountsRepository, App.CustomerCredit.Infraestructure.CreditAccountsEFRepository>();
            services.AddTransient<App.CustomerCredit.Domain.CreditAccountsVtexRepository, App.CustomerCredit.Infraestructure.CreditAccountsVtexRepository>();
            services.AddTransient<App.CustomerCredit.Domain.CreditAccountsSiesaRepository, App.CustomerCredit.Infraestructure.CreditAccountsSiesaRepository>();
            //Dependencies Injections Orders
            services.AddTransient<App.Orders.Domain.OrdersRepository, App.Orders.Infraestructure.OrdersEFRepository>();
            services.AddTransient<App.Orders.Domain.OrdersVtexRepository, App.Orders.Infraestructure.OrdersVtexRepository>();
            services.AddTransient<App.Orders.Domain.OrdersSiesaRepository, App.Orders.Infraestructure.OrdersSiesaRepository>();
            //Dependencies Injections SiesaOrders
            services.AddTransient<App.Orders.SiesaOrders.Domain.SiesaOrdersRepository, App.Orders.SiesaOrders.Infraestructure.SiesaOrdersEFRepository>();

            //Dependencies Injections Shared
            services.AddTransient<ILogs, ProcessLogs>();
            services.AddTransient<EmailSender, GmailSender>();

            //Scheduled Tasks
            services.AddHostedService<ScheduledRenderBrands>();
            //services.AddHostedService<ScheduledUpdateBrandsState>();
            //services.AddHostedService<ScheduledUpBrandsToVtex>();

            services.AddHostedService<ScheduledRenderCategories>();
            //services.AddHostedService<ScheduledUpCategoriesToVtex>();
            //services.AddHostedService<ScheduledUpdateCategoriesState>();

            //services.AddHostedService<ScheduledRenderProductsAndSkus>();
            //services.AddHostedService<ScheduledUpToVtexNullProductsAndSkus>();
            //services.AddHostedService<ScheduledUpdateProductsAndSkusStates>();

            //services.AddHostedService<ScheduledRenderPrices>();

            //services.AddHostedService<ScheduledRenderWarehouses>();

            //services.AddHostedService<ScheduledRenderInventories>();

            //services.AddHostedService<ScheduledRenderPromotions>();
            //services.AddHostedService<ScheduledUpToVtexNullPromotions>();

            //services.AddHostedService<ScheduledRenderCreditAccounts>();

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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
