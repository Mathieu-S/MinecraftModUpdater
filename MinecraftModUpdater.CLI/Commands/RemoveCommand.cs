using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Exceptions;
using CliFx.Infrastructure;
using MinecraftModUpdater.CLI.Adapters;
using MinecraftModUpdater.Core.Models.MMU;
using MinecraftModUpdater.Core.Services;
using Spectre.Console;

namespace MinecraftModUpdater.CLI.Commands
{
    [Command("remove", Description = "Delete a mod by its name or Id.")]
    public class RemoveCommand : ICommand
    {
        private readonly IModService _modService;
        private readonly IModListFileService _modListFileService;

        [CommandParameter(0, Name = "name", Description = "Mod name. For a mod with more than one word, use quotes." )]
        public string Name { get; set; } = "";
        
        [CommandOption("id", 'i', Description = "Indicates that the name is an Id.")]
        public bool IsId { get; set; } = false;
        
        public RemoveCommand(IModService modService, IModListFileService modListFileService)
        {
            _modService = modService ?? throw new ArgumentNullException(nameof(modService));
            _modListFileService = modListFileService ?? throw new ArgumentNullException(nameof(modListFileService));
        }
        
        public async ValueTask ExecuteAsync(IConsole console)
        {
            var ansiConsole = ConsoleAdapter.ConvertToAnsiConsole(ref console);

            var modListFile = await _modListFileService.ReadMinecraftModUpdaterFileAsync();
            ModData modToDelete;
            
            if (IsId)
            {
                try
                {
                    var modId = _modService.ConvertModId(Name);
                    modToDelete = modListFile.Mods.FirstOrDefault(x => x.Id == modId);
                }
                catch (Exception e)
                {
                    throw new CommandException(e.Message);
                }
            }
            else
            {
                var modsFound = modListFile.Mods.Where(m => m.Name.ToLower().Contains(Name.ToLower())).ToList();
                
                if (modsFound.Count == 1)
                {
                    modToDelete = modsFound.First();
                }
                else
                {
                    modToDelete = ansiConsole.Prompt(
                        new SelectionPrompt<ModData>()
                            .Title("The name entered is ambiguous. Several mods contain the same name. Please choose one:")
                            .PageSize(10)
                            .UseConverter(x => x.Name)
                            .AddChoices(modsFound));
                }
            }

            if (modToDelete == null)
            {
                ansiConsole.Render(new Markup("[red]This mod doesn't appear to be installed.[/]"));
                return;
            }
            
            _modService.DeleteModFile(modToDelete.FileName);
            await _modListFileService.RemoveModInModUpdaterFile(modToDelete);
            
            ansiConsole.Render(new Markup($"[lime]The mod [bold]{modToDelete.Name.EscapeMarkup()}[/] has been removed.[/]"));
        }
    }
}