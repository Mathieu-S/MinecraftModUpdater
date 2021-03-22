using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Exceptions;
using MinecraftModUpdater.CLI.Adapters;
using MinecraftModUpdater.Core.Models.Curse;
using MinecraftModUpdater.Core.Models.MMU;
using MinecraftModUpdater.Core.Services;
using Spectre.Console;

namespace MinecraftModUpdater.CLI.Commands
{
    [Command("add", Description = "Install a mod by its name or Id.")]
    public class AddCommand : ICommand
    {
        private readonly IModService _modService;
        private readonly IModListFileService _modListFileService;
        
        [CommandParameter(0, Name = "name", Description = "Mod name. For a mod with more than one word, use quotes." )]
        public string Name { get; set; } = "";
        
        [CommandOption("id", 'i', Description = "Indicates that the name is an Id.")]
        public bool IsId { get; set; } = false;

        public AddCommand(IModService modService, IModListFileService modListFileService)
        {
            _modService = modService ?? throw new ArgumentNullException(nameof(modService));
            _modListFileService = modListFileService ?? throw new ArgumentNullException(nameof(modListFileService));
        }
        
        public async ValueTask ExecuteAsync(IConsole console)
        {
            var ansiConsole = ConsoleAdapter.ConvertToAnsiConsole(ref console);
            
            var modListFile = await _modListFileService.ReadMinecraftModUpdaterFileAsync();
            CurseMod mod;
            CurseModFile modFile;
            
            if (IsId)
            {
                try
                {
                    var modId = _modService.ConvertModId(Name);
                    mod = await _modService.SearchByIdAsync(modId);
                    modFile = await _modService.GetLastCompatibleRelease(mod.Id, modListFile.MinecraftVersion);
                }
                catch (Exception e)
                {
                    throw new CommandException(e.Message);
                }
            }
            else
            {
                var modsFound = (List<CurseMod>) await _modService.SearchByNameAsync(Name);
                
                if (!modsFound.Any())
                {
                    ansiConsole.Render(new Markup($"[red]The mod called [bold]{Name}[/] is not found.[/]"));
                    return;
                }

                if (modsFound.Count == 1)
                {
                    mod = modsFound.First();
                    modFile = await _modService.GetLastCompatibleRelease(mod.Id, modListFile.MinecraftVersion);
                }
                else
                {
                    mod = ansiConsole.Prompt(
                        new SelectionPrompt<CurseMod>()
                            .Title("The name entered is ambiguous. Several mods contain the same name. Please choose one:")
                            .PageSize(10)
                            .UseConverter(x => x.Name)
                            .AddChoices(modsFound));
                    
                    modFile = await _modService.GetLastCompatibleRelease(mod.Id, modListFile.MinecraftVersion);
                }
            }
            
            if (modFile == null)
            {
                ansiConsole.Render(new Markup($"[red][bold]{Name.EscapeMarkup()}[/] isn't compatible with your minecraft version.[/]"));
                return;
            }
            
            await _modService.DownloadModFileAsync(modFile);
            
            var modData = new ModData
            {
                Id = mod.Id,
                Name = mod.Name,
                FileName = modFile.FileName,
                Version = modFile.Id
            };
            
            await _modListFileService.AddModToModUpdaterFile(modData);
            ansiConsole.Render(new Markup($"[lime][bold]{modData.Name.EscapeMarkup()}[/] has been installed.[/]"));
        }
    }
}