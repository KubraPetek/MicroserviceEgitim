using FreeCource.Service.Catalog.Services;
using FreeCource.Service.Catalog.Settings;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCource.Service.Catalog
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
            //Bu k?s?m CVatalog api yi Identity server ile koruma alt?na almak i?in eklendi
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Authority = Configuration["IdentityServerURL"];
                options.Audience = "resorce_catalog";
                options.RequireHttpsMetadata = false;
            });

            services.AddScoped<ICategoryService, CategoryService>();// ICategoryService ald???nda CategoryService d?nd?r --Dolu bir kategor servis d?necek
            services.AddScoped<ICourseService, CourseService>();

            services.AddAutoMapper(typeof(Startup));//Bu classa ba?l? t?m mapperlar? tarayacak
            services.AddControllers(opt=> {

                opt.Filters.Add(new AuthorizeFilter());//T?m controller lara tek tek Authorize eklemek yerine burdan tek seferde eklemi? olduk
            });


            //appsettingden database bilgilerini okumak i?in 

            services.Configure<DatabaseSettings>(Configuration.GetSection("DatabaseSetting"));
            services.AddSingleton<IDatabaseSettings>(sp =>
            {
                return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            });



            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FreeCource.Service.Catalog", Version = "v1" });
            });

            services.AddMassTransit(x =>
            {

                //DefaultPort : 5672
                x.UsingRabbitMq((contex, cfg) =>
                {
                    cfg.Host(Configuration["RabbitMQUrl"], "/", host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });
                });
            });

           // services.AddMassTransitHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreeCource.Service.Catalog v1"));
            }

            app.UseRouting();

            app.UseAuthentication();//art?k bu mikroservis koruma alt?nda 
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
