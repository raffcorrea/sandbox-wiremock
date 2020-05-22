using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SandBox.Catalog.API.ConfigurationOptions;
using SandBox.Catalog.API.Services;
using SandBox.Catalog.API.Services.Interfaces;

namespace SandBox.Catalog.API
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

            services.Configure<CepApiOptions>(Configuration.GetSection(nameof(CepApiOptions)));

            services.AddTransient<ICatalogService, CatalogService>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<ICepService, CepService>();

            services.AddSwaggerGen(c => {

                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Catalog API",
                        Version = "v1",
                        Description = "API created to test wiremock lib",
                        Contact = new OpenApiContact
                        {
                            Name = "Sandbox Test API Sample"
                        }
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog API V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
