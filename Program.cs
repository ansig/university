using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ContosoUniversity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            CreateDbIfNotExists(host);
            
            host.Run();
        }

        private static void CreateDbIfNotExists(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<SchoolContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // .ConfigureAppConfiguration((context, config) =>
                // {
                //     if (context.HostingEnvironment.IsProduction())
                //     {
                //         var builtConfig = config.Build();
                //
                //         var azureServiceTokenProvider = new AzureServiceTokenProvider();
                //         var keyVaultClient = new KeyVaultClient(
                //             new KeyVaultClient.AuthenticationCallback(
                //                 azureServiceTokenProvider.KeyVaultTokenCallback));
                //
                //         config.AddAzureKeyVault(
                //             $"https://{builtConfig["KeyVaultName"]}.vault.azure.net/",
                //             keyVaultClient,
                //             new DefaultKeyVaultSecretManager());
                //     }
                // })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
