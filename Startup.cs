using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Logging;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using AutoMapper;
using FluentValidation.AspNetCore;
using System.IdentityModel.Tokens;
using System;
using System.Text;

using sloflix.Data;
using sloflix.Models;
using sloflix.Helpers;
using sloflix.Helpers.Extensions;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using sloflix.Services;

namespace sloflix
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
      //   services.AddDbContextPool<DataContext>(options => options.UseMySql())
      services.AddDbContext<DataContext>(options =>
        options.UseMySql(Configuration.GetConnectionString("DefaultConnection"),
        mySqlOptions =>
        {
          mySqlOptions.ServerVersion(new Version(5, 7, 25), ServerType.MySql);
        })
      );

      services.AddSingleton<IJwtFactory, JwtFactory>();

      services.AddIdentity<AppUser, IdentityRole>()
        .AddEntityFrameworkStores<DataContext>()
        .AddDefaultTokenProviders();


      services.Configure<IdentityOptions>(options =>
      {
        // Password settings
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = false;

        // Lockout settings
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
        options.Lockout.MaxFailedAccessAttempts = 10;
        options.Lockout.AllowedForNewUsers = true;

        // User settings
        options.User.RequireUniqueEmail = true;
      });

      services.AddAutoMapper();

      services.AddMvc()
        .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>())
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

      // In production, the Angular files will be served from this directory
      services.AddSpaStaticFiles(configuration =>
      {
        configuration.RootPath = "ClientApp/dist";
      });

      // jwt wire up
      IdentityModelEventSource.ShowPII = true;
      var jwtAppSettingsOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
      var _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtAppSettingsOptions[nameof(JwtIssuerOptions.SecretKey)]));

      services.Configure<JwtIssuerOptions>(options =>
      {
        double validFor = 300.0;
        double.TryParse(jwtAppSettingsOptions[nameof(JwtIssuerOptions.ValidFor)], out validFor);

        options.Issuer = jwtAppSettingsOptions[nameof(JwtIssuerOptions.Issuer)];
        options.Audience = jwtAppSettingsOptions[nameof(JwtIssuerOptions.Audience)];
        options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
        options.ValidFor = TimeSpan.FromSeconds(validFor);
      });


      var tokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidIssuer = jwtAppSettingsOptions[nameof(JwtIssuerOptions.Issuer)],

        ValidateAudience = true,
        ValidAudience = jwtAppSettingsOptions[nameof(JwtIssuerOptions.Audience)],

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = _signingKey,

        RequireExpirationTime = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
      };

      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(options =>
      {
        // options.Audience = jwtAppSettingsOptions[nameof(JwtIssuerOptions.Audience)];
        options.ClaimsIssuer = jwtAppSettingsOptions[nameof(JwtIssuerOptions.Issuer)];
        options.TokenValidationParameters = tokenValidationParameters;
        options.SaveToken = true;
      });

      services.AddAuthorization(options =>
        options.AddPolicy("ApiUser",
          policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol,
                                        Constants.Strings.JwtClaims.ApiAccess)
        )
      );

      services.AddScoped<IWatchlistService, WatchlistService>();
      services.AddScoped<IMovieService, MovieService>();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      // if (env.IsDevelopment())
      // {
      //   app.UseDeveloperExceptionPage();
      // }
      // else
      {
        // app.UseExceptionHandler("/Error");
        // // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        // app.UseHsts();
        app.UseExceptionHandler(builder =>
        {
          builder.Run(async context =>
          {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            var error = context.Features.Get<IExceptionHandlerFeature>();
            if (error != null)
            {
              context.Response.AddApplicationError(error.Error.Message);
              await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
            }
          });
        });
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();
      app.UseSpaStaticFiles();

      app.UseAuthentication();

      app.UseMvc(routes =>
      {
        routes.MapRoute(
                  name: "default",
                  template: "{controller}/{action=Index}/{id?}");
      });

      app.UseSpa(spa =>
      {
        // To learn more about options for serving an Angular SPA from ASP.NET Core,
        // see https://go.microsoft.com/fwlink/?linkid=864501

        spa.Options.SourcePath = "ClientApp";

        if (env.IsDevelopment())
        {
          spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
        }
      });
    }
  }
}
