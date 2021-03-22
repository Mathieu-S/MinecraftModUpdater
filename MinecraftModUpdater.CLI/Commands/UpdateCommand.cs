using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using MinecraftModUpdater.CLI.Adapters;
using MinecraftModUpdater.Core.Models.MMU;
using MinecraftModUpdater.Core.Services;
using Spectre.Console;

namespace MinecraftModUpdater.CLI.Commands
{
    [Command("update", Description = "Update all mods or a specific one.")]
    public class UpdateCommand : ICommand
    {
        private readonly IModService _modService;
        private readonly IModListFileService _modListFileService;

        [CommandOption("id", 'i', Description = "Id of mod to update.")]
        public uint ModId { get; set; } = 0;
        
        [CommandOption("name", 'n', Description = "Name of mod to update.")]
        public string ModName { get; set; } = "";
        
        public UpdateCommand(IModService modService, IModListFileService modListFileService)
        {
            _modService = modService ?? throw new ArgumentNullException(nameof(modService));
            _modListFileService = modListFileService ?? throw new ArgumentNullException(nameof(modListFileService));
        }
        
        public async ValueTask ExecuteAsync(IConsole console)
        {
            var ansiConsole = ConsoleAdapter.ConvertToAnsiConsole(ref console);
            var modListFile = await _modListFileService.ReadMinecraftModUpdaterFileAsync();

            if (ModId != 0 || !string.IsNullOrWhiteSpace(ModName))
            {
                ModData modToUpdate = null;

                if (ModId != 0)
                {
                    modToUpdate = modListFile.Mods.FirstOrDefault(m => m.Id == ModId);
                }

                if (!string.IsNullOrWhiteSpace(ModName))
                {
                    var modsFound = (List<ModData>) modListFile.Mods.Where(m => m.Name.ToLower().Contains(ModName.ToLower()));
                    
                    if (!modsFound.Any())
                    {
                        ansiConsole.Render(new Markup($"[red]The mod called [bold]{ModName.EscapeMarkup()}[/] is not found.[/]"));
                        return;
                    }

                    if (modsFound.Count == 1)
                    {
                        modToUpdate = modsFound.First();
                    }
                    else
                    {
                        modToUpdate = ansiConsole.Prompt(
                            new SelectionPrompt<ModData>()
                                .Title("The name entered is ambiguous. Several mods contain the same name. Please choose one:")
                                .PageSize(10)
                                .UseConverter(x => x.Name)
                                .AddChoices(modsFound));
                    }
                }

                if (modToUpdate != null)
                {
                    var modFile = await _modService.GetLastCompatibleRelease(modToUpdate.Id, modListFile.MinecraftVersion);
                
                    if (modToUpdate.Version != modFile.Id)
                    {
                        await _modService.DownloadModFileAsync(modFile);
                        _modService.DeleteModFile(modToUpdate.FileName);
                        modToUpdate.Version = modFile.Id;
                        modToUpdate.FileName = modFile.FileName;
                        await _modListFileService.UpdateModInModUpdaterFile(modToUpdate);
                        
                        ansiConsole.Render(new Markup($"[lime][bold]{modToUpdate.Name.EscapeMarkup()}[/] has been updated.[/]"));
                    }
                    else
                    {
                        ansiConsole.Render(new Markup($"[yellow][bold]{modToUpdate.Name.EscapeMarkup()}[/] is already up to date.[/]"));
                    }
                }
                else
                {
                    ansiConsole.Render(new Markup("[red]This mod doesn't appear to be installed.[/]"));
                }
            }
            else
            {
                var updatedMods = new List<ModData>();
                
                foreach (var mod in modListFile.Mods)
                {
                    var modFile = await _modService.GetLastCompatibleRelease(mod.Id, modListFile.MinecraftVersion);
                
                    if (mod.Version != modFile.Id)
                    {
                        await _modService.DownloadModFileAsync(modFile);
                        _modService.DeleteModFile(mod.FileName);
                        mod.Version = modFile.Id;
                        mod.FileName = modFile.FileName;
                        await _modListFileService.UpdateModInModUpdaterFile(mod);
                        updatedMods.Add(mod);
                    }
                }

                if (!updatedMods.Any())
                {
                    ansiConsole.Render(new Markup("[yellow]There are no mods to update.[/]"));
                    return;
                }
                
                ansiConsole.Render(new Markup($"[lime]These [bold]{updatedMods.Count}[/] mod(s) have been updated:[/]"));
                
                var table = new Table();
                table.AddColumn(new TableColumn("Project ID").Centered());
                table.AddColumn(new TableColumn("Mod Name"));

                foreach (var mod in updatedMods)
                {
                    table.AddRow(mod.Id.ToString(), mod.Name.EscapeMarkup());
                }

                ansiConsole.Render(table);
            }
        }
    }
}