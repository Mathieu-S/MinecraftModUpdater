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
                // var actualPath = AppDomain.CurrentDomain.BaseDirectory;
                const string actualPath = @"E:\Bureau\Sandbox\";
                
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
                                    var modFile = await modService.GetLastCompatibleRelease(modsFound.FirstOrDefault().Id);
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
                        Console.WriteLine("Case 1");
                        break;

                    // Command to install all mods in modList.json
                    case ("uninstall"):
                    case ("remove"):
                    case ("rm"):
                    case ("r"):
                    case ("un"):
                    case ("unlink"):
                        Console.WriteLine("Case 1");
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
