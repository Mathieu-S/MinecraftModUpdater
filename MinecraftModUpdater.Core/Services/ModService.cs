using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MinecraftModUpdater.Core.Models.Curse;
using MinecraftModUpdater.Core.Repositories;

namespace MinecraftModUpdater.Core.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ModService
    {
        private readonly string _path;
        public IEnumerable<CurseMod> Mods { get; private set; }
        public string MinecraftVersion { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModService"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public ModService(string path)
        {
            _path = path;
            Mods = new List<CurseMod>();
        }

        /// <summary>
        /// Refreshes the mod list asynchronous.
        /// </summary>
        public async Task RefreshModListAsync()
        {
            Mods = (IList<CurseMod>)await ModRepository.GetModsAsync();
        }

        /// <summary>
        /// Searches the name of the by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IEnumerable<CurseMod> SearchByName(string name)
        {
            return Mods.Where(m => m.Name.Contains(name)).ToList();
        }

        /// <summary>
        /// Searches the name of the by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="mods">The mods.</param>
        /// <returns></returns>
        public IEnumerable<CurseMod> SearchByName(string name, IEnumerable<CurseMod> mods)
        {
            return mods.Where(m => m.Name.Contains(name)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modId"></param>
        /// <returns></returns>
        public async Task<CurseModFile> GetLastCompatibleRelease(uint modId)
        {
            var modFiles = await ModRepository.GetModFilesAsync(modId);
            var compatibleMods = modFiles.Where(m => m.GameVersion.Contains(MinecraftVersion));
            return compatibleMods.OrderBy(m => m.FileDate.Ticks).LastOrDefault();
        }
        
        /// <summary>
        /// Gets the last compatible release.
        /// </summary>
        /// <param name="mod">The mod.</param>
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
    }
}
