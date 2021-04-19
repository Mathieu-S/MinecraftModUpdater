using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Exceptions;
using CliFx.Infrastructure;
using MinecraftModUpdater.CLI.Adapters;
using MinecraftModUpdater.Core.Exceptions;
using MinecraftModUpdater.Core.Models.Curse;
using MinecraftModUpdater.Core.Services;
using Spectre.Console;

namespace MinecraftModUpdater.CLI.Commands
{
    [Command("search", Description = "Search a mod by name.")]
    public class SearchCommand : ICommand
    {
        private readonly IModService _modService;

        [CommandParameter(0, Name = "name", Description = "Mod name" )]
        public string Name { get; set; } = "";
        
        [CommandOption("strict", 's', Description = "Displays results containing strictly all terms.")]
        public bool Strict { get; set; } = false;
        
        public SearchCommand(IModService modService)
        {
            _modService = modService ?? throw new ArgumentNullException(nameof(modService));
        }
        
        public async ValueTask ExecuteAsync(IConsole console)
        {
            var ansiConsole = ConsoleAdapter.ConvertToAnsiConsole(ref console);
            List<CurseMod> mods;
            
            try
            {
                mods = (List<CurseMod>) await _modService.SearchByNameAsync(Name, Strict);
            }
            catch (MinecraftModUpdaterException e)
            {
                throw new CommandException(e.Message);
            }

            if (!mods.Any())
            {
                ansiConsole.Render(new Markup($"[yellow]Sorry but your search terms \"{Name.EscapeMarkup()}\" didn't return not result.[/]"));
                return;
            }
            
            var table = new Table();
            table.AddColumn(new TableColumn("Project ID").Centered());
            table.AddColumn(new TableColumn("Mod Name"));

            foreach (var mod in mods)
            {
                table.AddRow(mod.Id.ToString(), mod.Name.EscapeMarkup());
            }

            ansiConsole.Render(table);
        }
    }
}