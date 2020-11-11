using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using JointOffice.Configuration;
using JointOffice.DbModel;
using Microsoft.EntityFrameworkCore;
using JointOffice.Models;
using static JointOffice.WorkFlow.WF_Flow;
using Microsoft.AspNetCore.Http.Features;

namespace JointOffice
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //配置跨域处理
            services.AddCors(options =>
            {
                options.AddPolicy("any", builder =>
                {
                    builder.AllowAnyOrigin() //允许任何来源的主机访问
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();//指定处理cookie
                });
            });

            services.AddDbContext<JointOfficeContext>(options => options.UseSqlServer(Configuration.GetConnectionString("JointOfficeConnection")), ServiceLifetime.Transient);

            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddMemoryCache();
            services.AddMvc();
            services.AddSwaggerGen();
            services.AddOptions();
            services.AddTransient<IVerification, BVerification>();
            services.AddTransient<IPrincipalBase, PrincipalBase>();
            services.AddTransient<IBlobCunChu, BBlobCunChu>();
            services.AddTransient<IDynamic, BDynamic>();
            services.AddTransient<IWangPan, BWangPan>();
            services.AddTransient<ITeam, BTeam>();
            services.AddTransient<IContacts, BContacts>();
            services.AddTransient<IWork, BWork>();
            services.AddTransient<IEmail, BEmail>();
            services.AddTransient<INews, BNews>();
            services.AddTransient<IAttendance, BAttendance>();
            services.AddTransient<IAssessment, BAssessment>();
            services.AddTransient<IStatistical, BStatistical>();
            services.AddTransient<IWorkFlow, BWorkFlow>();
            services.AddTransient<ISaleRiQing, BSaleRiQing>();
            services.Configure<Root>(Configuration);
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSession();

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = 2147483647; //int.MaxValue
                x.MultipartBodyLengthLimit = 2147483647; //int.MaxValue
            });
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider svp)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //使用跨域
            app.UseCors("any");

            PrincipalBase.ServiceProvider = svp;

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUi();
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true
            });

            app.UseSession();
            MyHttpContext.ServiceProvider = svp;
        }
    }
}
