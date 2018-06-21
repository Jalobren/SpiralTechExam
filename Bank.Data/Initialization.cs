using Bank.AppLogic.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Data
{
    public static class Initialization
    {
        public static void InitIocContainer(IServiceCollection services)
        {
            services.AddTransient<IDatabase, Database>();
        }
    }
}
