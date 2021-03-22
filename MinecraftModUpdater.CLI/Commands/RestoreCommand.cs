using System;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using MinecraftModUpdater.CLI.Adapters;
using MinecraftModUpdater.Core.Services;
using Spectre.Console;

namespace MinecraftModUpdater.CLI.Commands
{
    [Command("restore", Description = "Restores the mods specified in a modlist.json.")]
    public class RestoreCommand : ICommand
    {
        private readonly IModService _modService;
        private readonly IModListFileService _modListFileService;

        public RestoreCommand(IModService modService, IModListFileService modListFileService)
        {
            _modService = modService ?? throw new ArgumentNullException(nameof(modService));
            _modListFileService = modListFileService ?? throw new ArgumentNullException(nameof(modListFileService));
        }
        
        public async ValueTask ExecuteAsync(IConsole console)
        {
            var ansiConsole = ConsoleAdapter.ConvertToAnsiConsole(ref console);
            var modListFile = await _modListFileService.ReadMinecraftModUpdaterFileAsync();
            
            if (modListFile.Mods.Count != 0)
            {
                foreach (var mod in modListFile.Mods)
                {
                    var modFile = await _modService.GetSpecificRelease(mod.Id, mod.Version);
                    await _modService.DownloadModFileAsync(modFile);
                }
                
                ansiConsole.Render(new Markup($"[lime]{modListFile.Mods.Count} mod(s) has been restored.[/]"));
            }
            else
            {
                ansiConsole.Render(new Markup("[yellow]There is no mods to install.[/]"));
            }
        }
    }
}