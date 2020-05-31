using System;
using System.Net;
using System.Text;
using CW2.DAL;
using CW2.Middlewares;
using CW2.ModelsEf;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace CW2
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
            services.AddDbContext<StudentsDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("StudentsDb")));
            services.AddSingleton<IDbService, SqlDbService>();
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ClockSkew = TimeSpan.Zero,
                            ValidateIssuer=true,
                            ValidateAudience=true,
                            ValidateLifetime=true,
                            ValidIssuer="Gakko",
                            ValidAudience="Students",
                            IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]))
                        };
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseMiddleware<LoggingMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.Use(async (context, next) =>
            //{
            //    var dbService = context.RequestServices.GetService<IDbService>();
            //    if (!context.Request.Headers.TryGetValue("Index", out var indexValue)
            //        || string.IsNullOrWhiteSpace(indexValue)
            //        || dbService.GetStudentByIndexNumber(indexValue) == null)
            //    {
            //        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //        return;
            //    }

            //    await next();
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
