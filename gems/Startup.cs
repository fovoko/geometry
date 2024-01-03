using Gems.SqliteDb.Db;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.IO;
using gems.common.Geometry.Figures;
using gems.common.Geometry.Calculators;

namespace gems
{
    /// <summary>
    /// Startup class, used to configure application during startup
    /// </summary>
    public class Startup()
    {
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ICalculator<Triangle>, CalculatorTriangle>();
            services.AddSingleton<ICalculator<Circle>, CalculatorCircle>();
            services.AddSingleton<ICalculateDispatcher, CalculatorDispatcher>();

            services.AddDbContext<FigureDbContext>();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Geometry figures API",
                    Description = "A geometry figures API demo",
                    Contact = new OpenApiContact
                    {
                        Name = "Fedor Kalugin",
                        Email = "fovoko@gmail.com"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

                //c.GeneratePolymorphicSchemas();
                c.UseOneOfForPolymorphism();
                c.UseAllOfForInheritance();

                c.ExampleFilters();
            });

            services.AddSwaggerExamplesFromAssemblyOf(typeof(CircleExample));
            services.AddSwaggerExamplesFromAssemblyOf(typeof(TriangleExample));
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="figureContext"></param>
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, FigureDbContext figureContext)
        {
            figureContext.Database.EnsureCreated();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Geometry figures API V1");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
