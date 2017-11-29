using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models.Base;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Services;
using AutoMapper;
using cetpaApi.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;

namespace cetpaApi
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
            services.AddMvc();

            // Add AutoMapper
            services.AddAutoMapper();

            // Add CORS
            services.AddCors(options =>
            {
                // Define one or more CORS policies
                /*options.AddPolicy("AllowSpecificOrigin", builder =>
                    builder.WithOrigins("http://localhost", "http://localhost:55723", Config.BaseUrl)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());*/

                options.AddPolicy("AllowSpecificOrigin", builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            //instantiate the data protection system at this folder
            string keyRingPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "temp-keys"));
            var protectionProvider = DataProtectionProvider.Create(new DirectoryInfo(keyRingPath));
            var dataProtector = protectionProvider.CreateProtector(
                "Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationMiddleware",
                Config.AppName,
                "v1");
            var ticketFormat = new TicketDataFormat(dataProtector);

            //Now configure the cookie options to have the same cookie name, and use the common format.
            services.AddAuthentication(Config.AppName)
                .AddCookie(Config.AppName, options =>
                {
                    options.AccessDeniedPath = "/Home/Index/";
                    options.LoginPath = "/Home/Index/";
                    options.TicketDataFormat = ticketFormat;
                }).AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Config.AppName,
                        ValidAudience = Config.AppName,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Config.JwtKey))
                    };
                });

            // Add exception filter
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new ExceptionFilterAttribute());
            });

            // Add EF
            var connection = Config.SqlConnection;
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<UserContext>(options => options.UseSqlServer(connection));

            // Add exception filter
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new ExceptionFilterAttribute());
            });

            // Add DI
            services.AddTransient<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}

            app.UseStaticFiles();

            //needed to get remote ip addresses
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                                   ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();

            //force https
            app.Use(async (context, next) =>
            {
                if (!context.Request.IsHttps)
                {
                    if (context.Request.Host.ToString().ToLower().IndexOf("localhost", StringComparison.Ordinal) == -1)
                    {
                        var withHttps = Config.BaseUrl;
                        context.Response.Redirect(withHttps + context.Request.Path);
                    }
                    else
                    {
                        await next();
                    }
                }
                else
                {
                    await next();
                }
            });

            //api routing
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
