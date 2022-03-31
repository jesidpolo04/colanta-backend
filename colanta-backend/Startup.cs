//Brands import
using colanta_backend.App.Brands.Jobs;
using colanta_backend.App.Brands.Domain;
using colanta_backend.App.Brands.Infraestructure;
using colanta_backend.App.Brands.Application;

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

            //DbContext
            services.AddDbContext<colantaContext>(options=> options.UseSqlServer("Server=localhost; Database=colanta; User=sa; Password=Jesing0408"));

            //Dependencies Injections Brands
            services.AddTransient<BrandsRepository, EFBrandsRepository>();
            services.AddTransient<BrandsVtexRepository, VtexBrandsRepository>();
            //---------------------------/

            //services.AddHostedService<ScheduledRenderBrands>();
            services.AddHostedService<ScheduledUpdateBrandsState>();
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
