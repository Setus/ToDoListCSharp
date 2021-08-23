﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ToDoList;
using ToDoList.integrationlayer;
using ToDoList.servicelayer;

namespace WebAPI
{
    public class Startup
    {

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {



            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins(GetAllowedOrigins())
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();
                                  });
            });
            services.AddControllers().AddNewtonsoftJson();

            services.AddScoped<IOperations, ItemOperations>();
            services.AddSingleton<IDBConnection, MySQLDBConnection>();
            services.AddSingleton<IDBConnection, MongoDBConnection>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private string[] GetAllowedOrigins()
        {
            return new string[] {
                AppConfigUtil.ReadDatabaseSetting("trustedUrl1", false),
                AppConfigUtil.ReadDatabaseSetting("trustedUrl2", false),
                AppConfigUtil.ReadDatabaseSetting("trustedUrl3", false)
            };
        }
    }
}
