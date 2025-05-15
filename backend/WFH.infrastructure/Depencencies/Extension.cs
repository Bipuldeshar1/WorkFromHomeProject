using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFH.infrastructure.Repository;
using WFH.infrastructure.Service;
using WorkFromHome.Application.Common.Repository;

namespace WFH.infrastructure.Depencencies
{
    public static class Extension
    {
        public static void Application(this IServiceCollection services)
        {

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddScoped<IToken, Token>();
            services.AddTransient<IEmailService, EmailService>();

        }
    }
}
