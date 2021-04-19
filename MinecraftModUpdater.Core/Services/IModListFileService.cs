using System;
using System.Threading.Tasks;
using MinecraftModUpdater.Core.Exceptions;
using MinecraftModUpdater.Core.Models.MMU;

namespace MinecraftModUpdater.Core.Services
{
    /// <summary>
    /// Service managing the dependency file.
    /// </summary>
    public interface IModListFileService
    {
        /// <summary>
        /// Creates the mod list file asynchronous.
        /// </summary>
        Task CreateModListFileAsync();
        
        /// <summary>
        /// Creates the mod list file asynchronous.
        /// </summary>
        /// <param name="minecraftVersion">The minecraft version.</param>
        /// <exception cref="ArgumentNullException">minecraftVersion</exception>
        /// <exception cref="MinecraftModUpdaterException">A file {FILE_NAME} already exist.</exception>
        Task CreateModListFileAsync(string minecraftVersion);
        
        /// <summary>
        /// Reads the minecraft mod updater file asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<ModListFile> ReadMinecraftModUpdaterFileAsync();
        
        /// <summary>
        /// Edits the minecraft mod updater file asynchronous.
        /// </summary>
        /// <param name="modListFile">The mod list file.</param>
        /// <exception cref="ArgumentNullException">modListFile</exception>
        Task EditMinecraftModUpdaterFileAsync(ModListFile modListFile);
        
        /// <summary>
        /// Adds the mod to mod updater file.
        /// </summary>
        /// <param name="mod">The mod.</param>
        /// <exception cref="ArgumentNullException">mod</exception>
        Task AddModToModUpdaterFile(ModData mod);
        
        /// <summary>
        /// Updates the mod in mod updater file.
        /// </summary>
        /// <param name="mod">The mod.</param>
        /// <exception cref="ArgumentNullException">mod</exception>
        Task UpdateModInModUpdaterFile(ModData mod);
        
        /// <summary>
        /// Removes the mod in mod updater file.
        /// </summary>
        /// <param name="mod">The mod.</param>
        /// <exception cref="ArgumentNullException">mod</exception>
        Task RemoveModInModUpdaterFile(ModData mod);
    }
}