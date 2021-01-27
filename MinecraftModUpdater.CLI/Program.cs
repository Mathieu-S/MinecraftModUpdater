using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinecraftModUpdater.Core.Exceptions;
using MinecraftModUpdater.Core.Models.Curse;
using MinecraftModUpdater.Core.Models.MMU;
using MinecraftModUpdater.Core.Services;

namespace MinecraftModUpdater.CLI
{
    /// <summary>
    /// Define the Minecraft Mod Updater CLI
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static async Task Main(string[] args)
        {
            if (args.Length != 0)
            {
                // Setup environment and services
                var actualPath = Environment.CurrentDirectory + @"\";
                var modListFileService = new ModListFileService(actualPath);
                var modService = new ModService(actualPath);
                
                switch (args[0])
                {
                    // Command to create a mod-list.json
                    case ("init"):
                        Console.WriteLine("Enter your minecraft version :");
                        var version = Console.ReadLine();

                        if (string.IsNullOrEmpty(version))
                        {
                            Console.WriteLine("Minecraft version unspecified.");
                            return;
                        }

                        try
                        {
                            await modListFileService.CreateModListFileAsync(version);
                        }
                        catch (MinecraftModUpdaterException e)
                        {
                            Console.WriteLine(e.Message);
                            return;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                        
                        Console.WriteLine($"You created a mod-list.json for Minecraft {version}");
                        break;

                    // Command to list all mods installed
                    case ("list"):
                    case ("ls"):
                    case ("la"):
                    case ("ll"):
                        var modList = await modListFileService.ReadMinecraftModUpdaterFileAsync();
                        
                        Console.WriteLine("MOD_ID    MOD_NAME");
                        
                        foreach (var mod in modList.Mods)
                        {
                            Console.WriteLine($"{mod.Id}    {mod.Name}");
                        }
                        
                        break;
                    
                    case ("search"):
                    case ("s"):
                    case ("se"):
                    case ("find"):
                        if (args.Length > 1 && args[1] != null)
                        {
                            var terms = (List<string>) GetParams(args);
                            string searchTerms;
                            List<CurseMod> mods;
                            
                            if (terms.Contains("--not-strict"))
                            {
                                terms.Remove("--not-strict");
                                searchTerms = string.Join(' ', terms);
                                mods = (List<CurseMod>) await modService.SearchByNameAsync(searchTerms, false);
                            }
                            else
                            {
                                searchTerms = string.Join(' ', terms);
                                mods = (List<CurseMod>) await modService.SearchByNameAsync(searchTerms);
                            }

                            if (!mods.Any())
                            {
                                Console.WriteLine("No mod found that matches your terms.");
                                return;
                            }
                            
                            Console.WriteLine($"{mods.Count} mod(s) correspond to your terms :");
                            Console.WriteLine("MOD_ID    MOD_NAME");

                            foreach (var mod in mods)
                            {
                                Console.WriteLine($"{mod.Id}    {mod.Name}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Missing mod name.");
                        }
                        
                        break;

                    // Command to install all mods or add one to modList.json
                    case ("install"):
                    case ("i"):
                    case ("add"):
                        if (args.Length > 1 && args[1] != null)
                        {
                            var modListFile = await modListFileService.ReadMinecraftModUpdaterFileAsync();
                            CurseMod mod;
                            CurseModFile modFile;

                            try
                            {
                                var modId = modService.ConvertModId(args[1]);
                                mod = await modService.SearchByIdAsync(modId);
                                modFile = await modService.GetLastCompatibleRelease(mod.Id, modListFile.MinecraftVersion);
                            }
                            catch (MinecraftModUpdaterException)
                            {
                                var terms = (List<string>) GetParams(args);
                                var searchTerms = string.Join(' ', terms);
                                var modsFound = (List<CurseMod>) await modService.SearchByNameAsync(searchTerms);

                                if (!modsFound.Any())
                                {
                                    Console.WriteLine($"The mod called {args[1]} is not found.");
                                    return;
                                }

                                if (modsFound.Count == 1)
                                {
                                    mod = modsFound.First();
                                    modFile = await modService.GetLastCompatibleRelease(modsFound.First().Id, modListFile.MinecraftVersion);
                                }
                                else
                                {
                                    Console.WriteLine("The name entered is ambiguous. Several mods contain the same name. Please choose one :");

                                    for (var i = 0; i < modsFound.Count; i++)
                                    {
                                        Console.WriteLine($"{i + 1}: {modsFound[i].Name}");
                                    }

                                    byte modSelectedIndex;
                                    var entrySelected = Console.ReadLine();

                                    if (string.IsNullOrWhiteSpace(entrySelected))
                                    {
                                        Console.WriteLine("Empty response. Exiting.");
                                        return;
                                    }

                                    try
                                    {
                                        modSelectedIndex = Convert.ToByte(entrySelected);
                                    }
                                    catch (FormatException)
                                    {
                                        Console.WriteLine($"{entrySelected} is not a number. Exiting.");
                                        return;
                                    }
                                    catch (OverflowException)
                                    {
                                        Console.WriteLine($"{entrySelected} don't correspond to a valid number. Exiting.");
                                        return;
                                    }

                                    try
                                    {
                                        mod = modsFound[modSelectedIndex - 1];
                                    }
                                    catch (ArgumentOutOfRangeException)
                                    {
                                        Console.WriteLine($"{entrySelected} don't correspond to a valid number. Exiting.");
                                        return;
                                    }

                                    modFile = await modService.GetLastCompatibleRelease(mod.Id, modListFile.MinecraftVersion);
                                }
                            }

                            if (modFile == null)
                            {
                                Console.WriteLine($"{mod.Name} isn't compatible with your minecraft version.");
                                return;
                            }
                            
                            await modService.DownloadModFileAsync(modFile);

                            var modData = new ModData
                            {
                                Id = mod.Id,
                                Name = mod.Name,
                                FileName = modFile.FileName,
                                Version = modFile.Id
                            };
                            
                            await modListFileService.AddModToModUpdaterFile(modData);
                            Console.WriteLine($"{modData.Name} has been installed.");
                        }
                        else
                        {
                            var modListFile = await modListFileService.ReadMinecraftModUpdaterFileAsync();

                            if (modListFile.Mods.Count != 0)
                            {
                                foreach (var mod in modListFile.Mods)
                                {
                                    var modFound = await modService.SearchByIdAsync(mod.Id);
                                    var modFile = await modService.GetLastCompatibleRelease(modFound.Id, modListFile.MinecraftVersion);
                                    await modService.DownloadModFileAsync(modFile);
                                }
                                
                                Console.WriteLine($"{modListFile.Mods.Count} mods has been installed.");
                            }
                            else
                            {
                                Console.WriteLine("There is no mods to install.");
                            }
                        }
                        
                        break;

                    // Command to update all mods in modList.json
                    case ("update"):
                    case ("up"):
                    case ("upgrade"):
                        if (args.Length > 1 && args[1] != null)
                        {
                            ModData modToUpdate;
                            var modListFile = await modListFileService.ReadMinecraftModUpdaterFileAsync();
                            
                            try
                            {
                                var modId = modService.ConvertModId(args[1]);
                                modToUpdate = modListFile.Mods.FirstOrDefault(m => m.Id == modId);
                            }
                            catch (MinecraftModUpdaterException)
                            {
                                modToUpdate = modListFile.Mods.FirstOrDefault(m => m.Name.Contains(args[1]));
                            }

                            if (modToUpdate != null)
                            {
                                var modFile = await modService.GetLastCompatibleRelease(modToUpdate.Id, modListFile.MinecraftVersion);

                                if (modToUpdate.Version != modFile.Id)
                                {
                                    await modService.DownloadModFileAsync(modFile);
                                    modService.DeleteModFile(modToUpdate.FileName);

                                    modToUpdate.Version = modFile.Id;
                                    modToUpdate.FileName = modFile.FileName;
                                    await modListFileService.UpdateModInModUpdaterFile(modToUpdate);
                                }
                            }
                        }
                        else
                        {
                            var modListFile = await modListFileService.ReadMinecraftModUpdaterFileAsync();
                            
                            foreach (var mod in modListFile.Mods)
                            {
                                var modFile = await modService.GetLastCompatibleRelease(mod.Id, modListFile.MinecraftVersion);

                                if (mod.Version != modFile.Id)
                                {
                                    await modService.DownloadModFileAsync(modFile);
                                    modService.DeleteModFile(mod.FileName);

                                    mod.Version = modFile.Id;
                                    mod.FileName = modFile.FileName;
                                    await modListFileService.UpdateModInModUpdaterFile(mod);
                                }
                            }
                        }
                        
                        break;

                    // Command to delete a mod in modList.json
                    case ("uninstall"):
                    case ("remove"):
                    case ("rm"):
                    case ("r"):
                    case ("un"):
                    case ("unlink"):
                        if (args.Length > 1 && args[1] != null)
                        {
                            var modListFile = await modListFileService.ReadMinecraftModUpdaterFileAsync();
                            ModData modToDelete;

                            try
                            {
                                var modId = modService.ConvertModId(args[1]);
                                modToDelete = modListFile.Mods.FirstOrDefault(m => m.Id == modId);
                            }
                            catch (MinecraftModUpdaterException)
                            {
                                modToDelete = modListFile.Mods.FirstOrDefault(m => m.Name.Contains(args[1]));
                            }

                            if (modToDelete == null)
                            {
                                Console.WriteLine($"The mod called {args[1]} is not found.");
                                return;
                            }
                            
                            modService.DeleteModFile(modToDelete.FileName);
                            await modListFileService.RemoveModInModUpdaterFile(modToDelete);
                        }
                        else
                        {
                            Console.WriteLine("Missing mod name or id.");
                        }
                        
                        Console.WriteLine($"The mod {args[1]} has been removed.");
                        break;

                    // Unknown command
                    default:
                        Console.WriteLine("Unknown command");
                        break;
                }
            }
            else
            {
                Console.WriteLine("No command");
            }
        }

        private static IEnumerable<string> GetParams(IEnumerable<string> args)
        {
            var terms = args.ToList();
            terms.RemoveAt(0);
            return terms;
        }
    }
}
