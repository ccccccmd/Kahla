﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Kahla.Server.Data;
using Kahla.Server.Models;
using Kahla.Server.Services;
using Aiursoft.Pylon;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.HttpOverrides;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Aiursoft.Pylon.Services.ToStargateServer;
using Aiursoft.Pylon.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Aiursoft.Pylon.Models;
using Aiursoft.Pylon.Services.ToAPIServer;
using Aiursoft.Pylon.Services.ToOSSServer;
using Kahla.Server.Middlewares;

namespace Kahla.Server
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public SameSiteMode Mode { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Mode = Convert.ToBoolean(configuration["LaxCookie"]) ? SameSiteMode.Lax : SameSiteMode.None;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureLargeFileUploadable();
            services.AddDbContext<KahlaDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnection")));

            services.AddIdentity<KahlaUser, IdentityRole>()
                .AddEntityFrameworkStores<KahlaDbContext>()
                .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(t => t.Cookie.SameSite = Mode);

            services.AddMvc();
            services.AddAiursoftAuth<KahlaUser>();

            services.AddScoped<ChannelService>();
            services.AddScoped<UserService>();
            services.AddScoped<SecretService>();
            services.AddScoped<PushMessageService>();
            services.AddScoped<PushKahlaMessageService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseEnforceHttps();
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseAiursoftAuthenticationFromConfiguration(Configuration, "Kahla");
            app.UseMiddleware<HandleKahlaOptionsMiddleware>();
            app.UseAuthentication();
            app.UseLanguageSwitcher();
            app.UseMvcWithDefaultRoute();
            app.UseDocGenerator();
        }
    }
}
