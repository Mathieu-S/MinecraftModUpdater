using System;
using System.Threading.Tasks;
using CliFx;
using Microsoft.Extensions.DependencyInjection;
using MinecraftModUpdater.CLI.Commands;
using MinecraftModUpdater.Core.Services;

namespace MinecraftModUpdater.CLI
{
    /// <summary>
    /// Define the Minecraft Mod Updater CLI
    /// </summary>
    public static class Program
    {
        private static IServiceProvider GetServiceProvider()
        {
            var actualPath = Environment.CurrentDirectory + @"\";
            var services = new ServiceCollection();

            // Register services
            services.AddSingleton<IModService>(new ModService(actualPath));
            services.AddSingleton<IModListFileService>(new ModListFileService(actualPath));

            // Register commands
            services.AddTransient<AddCommand>();
            services.AddTransient<InitCommand>();
            services.AddTransient<ListCommand>();
            services.AddTransient<RemoveCommand>();
            services.AddTransient<RestoreCommand>();
            services.AddTransient<SearchCommand>();
            services.AddTransient<UpdateCommand>();

            return services.BuildServiceProvider();
        }
        
        public static async Task<int> Main() =>
            await new CliApplicationBuilder()
                .AddCommandsFromThisAssembly()
                .SetTitle("Minecraft Mod Updater")
                .SetVersion("0.4.0")
                .SetDescription("A package manager for Minecraft's mods")
                .SetExecutableName("mmu")
                .UseTypeActivator(GetServiceProvider().GetRequiredService)
                .Build()
                .RunAsync();
    }
}
