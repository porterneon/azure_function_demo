using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AzureFunctionDemo.Dal;
using AzureFunctionDemo.Dal.Interfaces;
using AzureFunctionDemo.Dal.Services;

[assembly: FunctionsStartup(typeof(AzureFunctionDemo.Startup))]

namespace AzureFunctionDemo
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var operationDbCs = Environment.GetEnvironmentVariable("OperationDbCs", EnvironmentVariableTarget.Process);
            builder.Services.AddDbContext<OperationDbContext>(options =>
            {
                if (operationDbCs != null) options.UseSqlServer(operationDbCs);
            });

            builder.Services.AddTransient<IOperationDbContext, OperationDbContext>();
            builder.Services.AddTransient<IUserProfileService, UserProfileService>();
        }
    }
}