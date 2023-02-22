using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EksizSozlukClone.Infrastructure.Persistence.Extensions
{
    public static class Registration
    {
        public static IServiceCollection AddInfrastructreRegistration(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<EksiSozlukCloneContext>(options =>
            {
                var conStr = configuration["EksiSozlukCloneConnectionString"].ToString();
                options.UseSqlServer(conStr, opt =>
                {
                    opt.EnableRetryOnFailure();
                });
            });


       //     var seedData = new SeedData();
         //   seedData.SeedAsync(configuration).GetAwaiter().GetResult();
            return services;
        }
    }
}
