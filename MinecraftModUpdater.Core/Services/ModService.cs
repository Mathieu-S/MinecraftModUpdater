using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MinecraftModUpdater.Core.Exceptions;
using MinecraftModUpdater.Core.Models.Curse;
using MinecraftModUpdater.Core.Repositories;

namespace MinecraftModUpdater.Core.Services
{
    /// <summary>
    /// Service managing mod files.
    /// </summary>
    public class ModService
    {
        private readonly string _path;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModService"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public ModService(string path)
        {
            _path = path;
        }

        /// <summary>
        /// Searches the mod by its identifier.
        /// </summary>
        /// <param name="modId">The mod identifier.</param>
        /// <returns></returns>
        public async Task<CurseMod> SearchByIdAsync(uint modId)
        {
            return await ModRepository.GetModAsync(modId);
        }

        /// <summary>
        /// Searches the mod by its name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="strictSearch">if set to <c>true</c> [strict search].</param>
        /// <returns></returns>
        public async Task<IEnumerable<CurseMod>> SearchByNameAsync(string name, bool strictSearch = true)
        {
            var mods = (List<CurseMod>) await ModRepository.SearchModByNameAsync(name);

            if (strictSearch)
            {
                foreach(var word in name.ToLower().Split(' '))
                {
                    mods = mods.Where(m => m.Name.ToLower().Contains(word)).ToList();
                }
            }

            return mods;
        }

        /// <summary>
        /// Gets the latest compatible version of the mod.
        /// </summary>
        /// <param name="modId">The mod identifier.</param>
        /// <param name="minecraftVersion">The minecraft version.</param>
        /// <returns></returns>
        public async Task<CurseModFile> GetLastCompatibleRelease(uint modId, string minecraftVersion)
        {
            var modFiles = await ModRepository.GetModFilesAsync(modId);
            var compatibleMods = modFiles.Where(m => m.GameVersion.Contains(minecraftVersion));
            return compatibleMods.OrderBy(m => m.FileDate.Ticks).LastOrDefault();
        }

        /// <summary>
        /// Downloads the mod file asynchronous.
        /// </summary>
        /// <param name="mod">The mod.</param>
        /// <returns></returns>
        public async Task<bool> DownloadModFileAsync(CurseModFile mod)
        {
            if (!Directory.Exists(_path + @"\mods"))
            {
                Directory.CreateDirectory(_path + @"\mods");
            }

            if (File.Exists(_path + mod.FileName))
            {
                File.Delete(_path + mod.FileName);
            }

            await using var fs = new FileStream(_path + @"mods\" + mod.FileName, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
            var data = await ModRepository.GetStreamModFileAsync(mod);
            await data.CopyToAsync(fs);

            return true;
        }

        /// <summary>
        /// Deletes the mod file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void DeleteModFile(string fileName)
        {
            if (File.Exists(_path + @"\mods\" + fileName))
            {
                File.Delete(_path + @"\mods\" +fileName);
            }
        }

        /// <summary>
        /// Converts the mod identifier.
        /// </summary>
        /// <param name="modId">The mod identifier.</param>
        /// <returns></returns>
        /// <exception cref="MinecraftModUpdaterException">The modId '{modId}' is not a valid ID.</exception>
        public uint ConvertModId(string modId)
        {
            try
            {
                return Convert.ToUInt32(modId);
            }
            catch (FormatException e)
            {
                throw new MinecraftModUpdaterException($"The modId '{modId}' is not a valid ID.", e.InnerException);
            }
        }
    }
}
