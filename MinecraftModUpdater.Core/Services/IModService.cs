using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MinecraftModUpdater.Core.Exceptions;
using MinecraftModUpdater.Core.Models.Curse;

namespace MinecraftModUpdater.Core.Services
{
    /// <summary>
    /// Service managing mod files.
    /// </summary>
    public interface IModService
    {
        /// <summary>
        /// Searches the mod by its identifier.
        /// </summary>
        /// <param name="modId">The mod identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">modId can't be 0. - modId</exception>
        Task<CurseMod> SearchByIdAsync(uint modId);
        
        /// <summary>
        /// Searches the mod by its name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="strictSearch">if set to <c>true</c> [strict search].</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">name</exception>
        /// <exception cref="MinecraftModUpdaterException"></exception>
        Task<IEnumerable<CurseMod>> SearchByNameAsync(string name, bool strictSearch = true);
        
        /// <summary>
        /// Gets the latest compatible version of the mod.
        /// </summary>
        /// <param name="modId">The mod identifier.</param>
        /// <param name="minecraftVersion">The minecraft version.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">modId can't be 0. - modId</exception>
        /// <exception cref="ArgumentNullException">minecraftVersion</exception>
        Task<CurseModFile> GetLastCompatibleRelease(uint modId, string minecraftVersion);
        
        /// <summary>
        /// Gets a specific version of the mod.
        /// </summary>
        /// <param name="modId">The mod identifier.</param>
        /// <param name="fileId">The file identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">
        /// modId can't be 0. - modId
        /// or
        /// fileId can't be 0. - fileId
        /// </exception>
        Task<CurseModFile> GetSpecificRelease(uint modId, uint fileId);
        
        /// <summary>
        /// Downloads the mod file asynchronous.
        /// </summary>
        /// <param name="mod">The mod.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">mod</exception>
        Task<bool> DownloadModFileAsync(CurseModFile mod);
        
        /// <summary>
        /// Deletes the mod file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <exception cref="ArgumentNullException">fileName</exception>
        void DeleteModFile(string fileName);
        
        /// <summary>
        /// Converts the mod identifier.
        /// </summary>
        /// <param name="modId">The mod identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">modId</exception>
        /// <exception cref="MinecraftModUpdaterException">The modId '{modId}' is not a valid ID.</exception>
        uint ConvertModId(string modId);
    }
}