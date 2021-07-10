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
    /// <inheritdoc/>
    public class ModService : IModService
    {
        private readonly string _path;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModService"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <exception cref="ArgumentNullException">path</exception>
        public ModService(string path)
        {
            _path = path ?? throw new ArgumentNullException(nameof(path));
        }

        /// <inheritdoc/>
        public async Task<CurseMod> SearchByIdAsync(uint modId)
        {
            _ = modId == 0 ? throw new ArgumentException("modId can't be 0.", nameof(modId)) : 0;
            return await ModRepository.GetModAsync(modId);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<CurseMod>> SearchByNameAsync(string name, bool strictSearch = true)
        {
            _ = name ?? throw new ArgumentNullException(nameof(name));
            
            List<CurseMod> mods;
            
            try
            {
                mods = (List<CurseMod>) await ModRepository.SearchModByNameAsync(name);
            }
            catch (CurseApiException e)
            {
                throw new MinecraftModUpdaterException(e.Message, e.InnerException);
            }

            if (strictSearch)
            {
                foreach(var word in name.ToLower().Split(' '))
                {
                    mods = mods.Where(m => m.Name.ToLower().Contains(word)).ToList();
                }
            }

            return mods;
        }

        /// <inheritdoc/>
        public async Task<CurseModFile> GetLastCompatibleRelease(uint modId, string minecraftVersion)
        {
            _ = modId == 0 ? throw new ArgumentException("modId can't be 0.", nameof(modId)) : 0;
            _ = minecraftVersion ?? throw new ArgumentNullException(nameof(minecraftVersion));
            
            var modFiles = await ModRepository.GetModFilesAsync(modId);
            var compatibleMods = modFiles.Where(m =>
                m.GameVersion.Contains(minecraftVersion) &&
                !m.GameVersion.Contains("Fabric")
            );

            if (!compatibleMods.Any())
            {
                var (majorVersion, minorVersion, patchVersion) = ParseMinecraftVersion(minecraftVersion);
                while (!compatibleMods.Any())
                {
                    patchVersion--;
                    compatibleMods = modFiles.Where(m => m.GameVersion.Contains($"{majorVersion}.{minorVersion}.{patchVersion}"));
                    
                    if (patchVersion < 0)
                    {
                        break;
                    }
                }
            }

            return compatibleMods.OrderBy(m => m.FileDate.Ticks).LastOrDefault();
        }

        /// <inheritdoc/>
        public async Task<CurseModFile> GetSpecificRelease(uint modId, uint fileId)
        {
            _ = modId == 0 ? throw new ArgumentException("modId can't be 0.", nameof(modId)) : 0;
            _ = fileId == 0 ? throw new ArgumentException("fileId can't be 0.", nameof(fileId)) : 0;
            
            var modFiles = await ModRepository.GetModFilesAsync(modId);
            return modFiles.FirstOrDefault(m => m.Id == fileId);
        }

        /// <inheritdoc/>
        public async Task<bool> DownloadModFileAsync(CurseModFile mod)
        {
            _ = mod ?? throw new ArgumentNullException(nameof(mod));
            
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

        /// <inheritdoc/>
        public void DeleteModFile(string fileName)
        {
            _ = fileName ?? throw new ArgumentNullException(nameof(fileName));
            
            if (File.Exists(_path + @"\mods\" + fileName))
            {
                File.Delete(_path + @"\mods\" +fileName);
            }
        }

        /// <inheritdoc/>
        public uint ConvertModId(string modId)
        {
            _ = modId ?? throw new ArgumentNullException(nameof(modId));
            
            try
            {
                return Convert.ToUInt32(modId);
            }
            catch (FormatException e)
            {
                throw new MinecraftModUpdaterException($"The modId '{modId}' is not a valid ID.", e.InnerException);
            }
        }

        /// <summary>
        /// Split the minecraft version string into tuple.
        /// </summary>
        /// <param name="minecraftVersion"></param>
        /// <returns></returns>
        private static (sbyte, sbyte, sbyte) ParseMinecraftVersion(string minecraftVersion)
        {
            var minecraftVersionNumberSplit = minecraftVersion.Split('.');
            return (Convert.ToSByte(minecraftVersionNumberSplit[0]),
                Convert.ToSByte(minecraftVersionNumberSplit[1]),
                Convert.ToSByte(minecraftVersionNumberSplit[2]));
        }
    }
}
