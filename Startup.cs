using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KKT_APP_FA.Models.DB;
using KKT_APP_FA.Services;
using KKT_APP_FA.Services.DB;
using KKT_APP_FA.Services.Helpers;
using KKT_APP_FA.Units;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading;

namespace KKT_APP_FA
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public static IConfiguration ConfigStatic { get; set; }

        MainCycle mainCycle;

        string log_file_name;

        //==============================================================================================================================================
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
            ConfigStatic = configuration;
            try { Program.MaxTransactionsInShift = Convert.ToInt32(configuration.GetSection("MainSettings")["MaxTransactionsInShift"]); } catch { Program.MaxTransactionsInShift = 5000; }
            try { Program.IsAutomaticDevice = (configuration.GetSection("MainSettings")["IsAutomaticDevice"].ToLower()) == "true" ? true : false; } catch { Program.IsAutomaticDevice = true; }
            log_file_name = "main_log";
        }

        //==============================================================================================================================================
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Transient сервисы:
            services.AddTransient<IBaseService, BaseService>();
            services.AddTransient<IDateTimeHelper, DateTimeHelper>();
            services.AddTransient<APIHelper>();
            services.AddTransient<SQLiteService>();

            // Singleton сервисы:
            services.AddSingleton<ILoggerHelper, LoggerHelper>();
            services.AddSingleton<ICRC32, CRC32>();
            //services.AddSingleton<MainCycle>();

            // Дефолтные сервисы:
            services.AddMvc();//.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            /// DB сервисы:
            //services.AddDbContext<DBRegistrationsService>(options => options.UseSqlite(this.Configuration.GetSection("ConnectionStrings")["LocalDBConnectionString"]));

        }

        //==============================================================================================================================================
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            ICRC32 cRC32, SQLiteService sQLiteService, APIHelper aPIHelper, ILoggerHelper logger, IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Задаем дефолтный маршрут:
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
            });

            // стартуем главный цикл:
            mainCycle = new MainCycle(cRC32, sQLiteService, aPIHelper, logger, configuration);
            logger.Write("Starting MainCycle Task...", log_file_name, false);
            mainCycle.Start();
            logger.Write("MainCycle Task started. Id = " + Program.MainTask.Id + ", Status: " + Program.MainTask.Status, log_file_name);

            // стартуем контрольный таск:
            Program.ControlTask = Task.Factory.StartNew(() => {
                TaskStatus status;
                while (true)
                {
                    status = Program.MainTask.Status;
                    if (status.ToString().ToLower() != "running") // если главный таск упал
                    {
                        logger.Write("MainCycle Task crashed!!! Status: " + Program.MainTask.Status, log_file_name, false);
                        // рестартуем главный таск:
                        logger.Write("Restarting MainCycle Task...", log_file_name, false);
                        mainCycle = new MainCycle(cRC32, sQLiteService, aPIHelper, logger, configuration);
                        mainCycle.Start();
                        logger.Write("MainCycle Task restarted. Id = " + Program.MainTask.Id + ", Status: " + Program.MainTask.Status, log_file_name);
                    }
                    //Console.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " MainTask status: " + status.ToString());
                    Thread.Sleep(1000);
                }
            });
        }

        //==============================================================================================================================================
    }
}
