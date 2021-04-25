using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Exceptions;
using CliFx.Infrastructure;
using MinecraftModUpdater.CLI.Adapters;
using MinecraftModUpdater.Core.Services;
using Spectre.Console;

namespace MinecraftModUpdater.CLI.Commands
{
    [Command("upgrade", Description = "Change the minecraft version stored in modlist.json.")]
    public class UpgradeCommand : ICommand
    {
        private readonly IModListFileService _modListFileService;

        public UpgradeCommand(IModListFileService modListFileService)
        {
            _modListFileService = modListFileService ?? throw new ArgumentNullException(nameof(modListFileService));
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            var ansiConsole = ConsoleAdapter.ConvertToAnsiConsole(ref console);
            var modListFile = await _modListFileService.ReadMinecraftModUpdaterFileAsync();
            
            var versionPattern = new Regex(@"^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)$");
            
            var version = ansiConsole.Ask<string>("Enter your minecraft version :");

            if (!versionPattern.IsMatch(version))
            {
                throw new CommandException("Your minecraft version don't match the version pattern. Please use XX.XX.XX");
            }

            modListFile.MinecraftVersion = version;
            await _modListFileService.EditMinecraftModUpdaterFileAsync(modListFile);
            
            ansiConsole.Render(new Markup($"Your Minecraft version has been change to [lime]{version}[/]"));
        }
    }
}