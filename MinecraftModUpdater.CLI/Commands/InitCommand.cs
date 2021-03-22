using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Exceptions;
using CliFx.Infrastructure;
using MinecraftModUpdater.CLI.Adapters;
using MinecraftModUpdater.Core.Exceptions;
using MinecraftModUpdater.Core.Services;
using Spectre.Console;

namespace MinecraftModUpdater.CLI.Commands
{
    [Command("init", Description = "Initialise a new modlist.json.")]
    public class InitCommand : ICommand
    {
        private readonly IModListFileService _modListFileService;
        
        public InitCommand(IModListFileService modListFileService)
        {
            _modListFileService = modListFileService ?? throw new ArgumentNullException(nameof(modListFileService));
        }
        
        public async ValueTask ExecuteAsync(IConsole console)
        {
            var ansiConsole = ConsoleAdapter.ConvertToAnsiConsole(ref console);
            var versionPattern = new Regex(@"^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)$");
            
            var version = ansiConsole.Ask<string>("Enter your minecraft version :");

            if (!versionPattern.IsMatch(version))
            {
                throw new CommandException("Your minecraft version don't match the version pattern. Please use XX.XX.XX");
            }
            
            try
            {
                await _modListFileService.CreateModListFileAsync(version);
            }
            catch (MinecraftModUpdaterException e)
            {
                throw new CommandException(e.Message);
            }
            
            ansiConsole.Render(new Markup($"You created a [italic]modlist.json[/] for [lime]Minecraft {version}[/]"));
        }
    }
}
