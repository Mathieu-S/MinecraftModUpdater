using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MinecraftModUpdater.Core.Exceptions;
using MinecraftModUpdater.Core.Models.MMU;

namespace MinecraftModUpdater.Core.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ModListFileService
    {
        private readonly string _path;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModListFileService"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public ModListFileService(string path)
        {
            _path = path;
        }

        /// <summary>
        /// Creates the mod list file asynchronous.
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
        /// Creates the mod list file asynchronous.
        /// </summary>
        /// <param name="minecraftVersion">The minecraft version.</param>
        /// <exception cref="MinecraftModUpdaterException">A file mod-list.json already exist.</exception>
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
        /// Reads the minecraft mod updater file asynchronous.
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
        /// Edits the minecraft mod updater file asynchronous.
        /// </summary>
        /// <param name="data">The data.</param>
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
        /// Adds the mod to mod updater file.
        /// </summary>
        /// <param name="mod">The mod.</param>
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
        /// Updates the mod in mod updater file.
        /// </summary>
        /// <param name="mod">The mod.</param>
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
        /// Removes the mod in mod updater file.
        /// </summary>
        /// <param name="mod">The mod.</param>
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