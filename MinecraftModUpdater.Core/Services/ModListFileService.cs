using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MinecraftModUpdater.Core.Exceptions;
using MinecraftModUpdater.Core.Models.MMU;

namespace MinecraftModUpdater.Core.Services
{
    public class ModListFileService
    {
        private readonly string _path;

        public ModListFileService(string path)
        {
            _path = path;
        }
        
        /// <summary>
        /// Creates the curse mod updater file.
        /// </summary>
        public async Task CreateModListFileAsync()
        {
            if (IsModListFileExist())
            {
                return;
            }

            var modListFile = new ModListFile()
            {
                FileVersion = 1,
                MinecraftVersion = "0",
                Mods = new List<ModData>()
            };

            await using var createStream = File.Create(_path + "modList.json");
            await JsonSerializer.SerializeAsync(createStream, modListFile);
        }
        
        /// <summary>
        /// Creates the curse mod updater file.
        /// </summary>
        /// <param name="minecraftVersion"></param>
        /// <returns></returns>
        public async Task CreateModListFileAsync(string minecraftVersion)
        {
            if (IsModListFileExist())
            {
                throw new MinecraftModUpdaterException("A file mod-list.json already exist.");
            }

            var modListFile = new ModListFile()
            {
                FileVersion = 1,
                MinecraftVersion = minecraftVersion,
                Mods = new List<ModData>()
            };

            await using var createStream = File.Create(_path + "modList.json");
            await JsonSerializer.SerializeAsync(createStream, modListFile);
        }

        /// <summary>
        /// Reads the curse mod updater file.
        /// </summary>
        /// <returns></returns>
        public async Task<ModListFile> ReadMinecraftModUpdaterFileAsync()
        {
            if (!IsModListFileExist())
            {
                await CreateModListFileAsync();
            }

            await using var openStream = File.OpenRead(_path + "modList.json");
            return await JsonSerializer.DeserializeAsync<ModListFile>(openStream);
        }

        /// <summary>
        /// Edits the curse mod updater file.
        /// </summary>
        /// <returns></returns>
        public async Task EditMinecraftModUpdaterFileAsync(ModListFile data)
        {
            if (!IsModListFileExist())
            {
                return;
            }

            await using var openWriteStream = new FileStream(_path + "modList.json", FileMode.Truncate);
            await JsonSerializer.SerializeAsync(openWriteStream, data);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public async Task AddModToModUpdaterFile(ModData mod)
        {
            if (!IsModListFileExist())
            {
                return;
            }

            var modUpdaterFile = await ReadMinecraftModUpdaterFileAsync();

            if (modUpdaterFile.Mods.Contains(mod))
            {
                return;
            }
            
            modUpdaterFile.Mods.Add(mod);
            
            await EditMinecraftModUpdaterFileAsync(modUpdaterFile);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public async Task UpdateModInModUpdaterFile(ModData mod)
        {
            if (!IsModListFileExist())
            {
                return;
            }

            var modUpdaterFile = await ReadMinecraftModUpdaterFileAsync();

            var modInFile = modUpdaterFile.Mods.FirstOrDefault(m => m.Name == mod.Name);
            modInFile.FileName = mod.FileName;
            modInFile.Version = mod.Version;

            await EditMinecraftModUpdaterFileAsync(modUpdaterFile);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public async Task RemoveModInModUpdaterFile(ModData mod)
        {
            if (!IsModListFileExist())
            {
                return;
            }

            var modUpdaterFile = await ReadMinecraftModUpdaterFileAsync();

            if (!modUpdaterFile.Mods.Contains(mod))
            {
                return;
            }
            
            modUpdaterFile.Mods.Remove(mod);
            
            await EditMinecraftModUpdaterFileAsync(modUpdaterFile);
        }

        /// <summary>
        /// Determines whether [is mod list file exist].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is mod list file exist]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsModListFileExist()
        {
            return File.Exists(_path + "modList.json");
        }
    }
}