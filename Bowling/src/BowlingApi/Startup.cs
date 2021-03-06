using BowlingApi.Models;
using BowlingApi.Repositories;
using BowlingApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BowlingApi
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
            var connectionString = Configuration.GetConnectionString("BowlingDatabase");
            services.AddDbContext<BowlingDBContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddControllers();

            services.AddScoped(typeof(IBowlingDBRepository), typeof(BowlingDBRepository));
            services.AddScoped(typeof(IDeleteEntitiesRepository), typeof(DeleteEntitiesRepository));
            services.AddScoped(typeof(IBowlingService), typeof(BowlingService));

            services.AddMvc().AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.PropertyNamingPolicy = null;//JsonNamingPolicy.CamelCase;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
