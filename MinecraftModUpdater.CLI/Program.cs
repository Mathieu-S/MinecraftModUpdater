using System;
using System.Linq;
using System.Threading.Tasks;
using MinecraftModUpdater.Core.Models.MMU;
using MinecraftModUpdater.Core.Services;

namespace MinecraftModUpdater.CLI
{
    /// <summary>
    /// 
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
                var actualPath = Environment.CurrentDirectory + @"\";
                var modListFileService = new ModListFileService(actualPath);
                var modService = new ModService(actualPath);
                
                switch (args[0])
                {
                    // Command to create a modList.json
                    case ("init"):
                        Console.WriteLine("Enter your minecraft version :");
                        var version = Console.ReadLine();

                        if (string.IsNullOrEmpty(version))
                        {
                            Console.WriteLine("Minecraft version unspecified.");
                            return;
                        }
                        
                        await modListFileService.CreateModListFileAsync(version);
                        break;

                    // Command to install all mods or add one to modList.json
                    case ("install"):
                    case ("i"):
                    case ("add"):
                        await modService.RefreshModListAsync();

                        if (args.Length > 1 && args[1] != null)
                        {
                            var modsFound = modService.SearchByName(args[1]);
                            var modFile = await modService.GetLastCompatibleRelease(modsFound.FirstOrDefault().Id, "1.16.4");

                            await modService.DownloadModFileAsync(modFile);

                            var mod = new ModData
                            {
                                Id = modsFound.FirstOrDefault().Id,
                                Name = modsFound.FirstOrDefault().Name,
                                FileName = modFile.FileName,
                                Version = modFile.Id
                            };
                            
                            await modListFileService.AddModToModUpdaterFile(mod);
                        }
                        else
                        {
                            var modListFile = await modListFileService.ReadMinecraftModUpdaterFileAsync();

                            if (modListFile.Mods.Count != 0)
                            {
                                foreach (var mod in modListFile.Mods)
                                {
                                    var modsFound = modService.SearchByName(mod.Name);
                                    var modFile = await modService.GetLastCompatibleRelease(modsFound.FirstOrDefault().Id, "1.16.4");
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
                        await modService.RefreshModListAsync();
                        
                        if (args.Length > 1 && args[1] != null)
                        {
                            var modListFile = await modListFileService.ReadMinecraftModUpdaterFileAsync();
                            var modToUpdate = modListFile.Mods.FirstOrDefault(m => m.Name == args[1]);

                            if (modToUpdate != null)
                            {
                                var modsFound = await modService.GetLastCompatibleRelease(modToUpdate.Id);
                                var modFile = await modService.GetLastCompatibleRelease(modsFound.Id, "1.16.4");

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
                                var modFile = await modService.GetLastCompatibleRelease(mod.Id, "1.16.4");

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
                            var modToDelete = modListFile.Mods. FirstOrDefault(m => m.Name.Contains(args[1]));
                            
                            modService.DeleteModFile(modToDelete.FileName);
                            await modListFileService.RemoveModInModUpdaterFile(modToDelete);
                        }
                        else
                        {
                            Console.WriteLine("Missing mod name.");
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
    }
}
