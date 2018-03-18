using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Paari.DataAccess;
using Paari.DataAccess.Repository;
using Paari.Infrastructure.Repository;
using Paari.Models;

namespace Paari
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IContainer ApplicationContainer { get; private set; }
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<ProductContext>(opt => opt.UseInMemoryDatabase());
            
            // Add framework services.
            services.AddMvc();

            //Autofac
            ContainerBuilder builder = new ContainerBuilder();
            builder.Populate(services);

            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder<ProductContext>();
            optionsBuilder.UseInMemoryDatabase();
            var options = (DbContextOptions<ProductContext>)optionsBuilder.Options;
            ProductContext dbContext = new ProductContext(options);

            builder.RegisterType<ProductRepository>().As<IProductRepository>().WithParameter("context", dbContext).SingleInstance();
            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
