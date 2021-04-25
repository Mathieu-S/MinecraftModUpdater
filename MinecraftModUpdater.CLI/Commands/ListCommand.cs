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
    [Command("list", Description = "List all installed mods.")]
    public class ListCommand : ICommand
    {
        private readonly IModListFileService _modListFileService;

        public ListCommand(IModListFileService modListFileService)
        {
            _modListFileService = modListFileService ?? throw new ArgumentNullException(nameof(modListFileService));
        }
        
        public async ValueTask ExecuteAsync(IConsole console)
        {
            var ansiConsole = ConsoleAdapter.ConvertToAnsiConsole(ref console);
            var modList = await _modListFileService.ReadMinecraftModUpdaterFileAsync();
            
            var table = new Table();
            table.AddColumn(new TableColumn("Project ID").Centered());
            table.AddColumn(new TableColumn("Mod Name"));

            foreach (var mod in modList.Mods)
            {
                table.AddRow(mod.Id.ToString(), mod.Name.EscapeMarkup());
            }

            ansiConsole.Write(table);
        }
    }
}