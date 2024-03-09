using HKDXX6_HFT_2023241.Endpoint.Services;
using HKDXX6_HFT_2023241.Logic;
using HKDXX6_HFT_2023241.Models.DBModels;
using HKDXX6_HFT_2023241.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HKDXX6_HFT_2023241.Endpoint
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<PoliceDbContext>();

            services.AddTransient<IRepository<Precinct>, PrecinctRepository>();
            services.AddTransient<IRepository<Officer>, OfficerRepository>();
            services.AddTransient<IRepository<Case>, CaseRepository>();

            services.AddTransient<IPrecinctLogic, PrecinctLogic>();
            services.AddTransient<IOfficerLogic, OfficerLogic>();
            services.AddTransient<ICaseLogic, CaseLogic>();

            services.AddSignalR();

            services.AddControllers().AddNewtonsoftJson();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HKDXX6_HFT_2023241.Endpoint", Version = "v1" });
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
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HKDXX6_HFT_2023241_API");
                c.RoutePrefix = string.Empty;
            });

            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var ex = context.Features
                            .Get<IExceptionHandlerPathFeature>()
                            .Error;
                var resp = new {
                    Type = ex.GetType().Name,
                    Msg = ex.Message 
                }; 
                await context.Response.WriteAsJsonAsync(resp);

            }));

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<SignalRHub>("/hub");
            });
        }
    }
}
