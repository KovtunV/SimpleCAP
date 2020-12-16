using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleCAP;

namespace SimpleCapTest.WebApp1
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSimpleCap(q =>
            {
                q.UseInMemoryStorage();
                q.UseKafka("localhost:9092");
            });

            // Optional
            // services.Configure<CallbackOptions>(q => q.Timeout = TimeSpan.FromMilliseconds(10));
        }

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
