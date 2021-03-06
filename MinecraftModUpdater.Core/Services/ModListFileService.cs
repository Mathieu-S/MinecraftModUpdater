﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MinecraftModUpdater.Core.Exceptions;
using MinecraftModUpdater.Core.Models.MMU;

namespace MinecraftModUpdater.Core.Services
{
    /// <inheritdoc/>
    public class ModListFileService : IModListFileService
    {
        private readonly string _path;
        private const string FILE_NAME = "modlist.json";

        /// <summary>
        /// Initializes a new instance of the <see cref="ModListFileService"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <exception cref="ArgumentNullException">path</exception>
        public ModListFileService(string path)
        {
            _path = path ?? throw new ArgumentNullException(nameof(path));
        }

        /// <inheritdoc/>
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

            await using var createStream = File.Create(_path + FILE_NAME);
            await JsonSerializer.SerializeAsync(createStream, modListFile);
        }

        /// <inheritdoc/>
        public async Task CreateModListFileAsync(string minecraftVersion)
        {
            _ = minecraftVersion ?? throw new ArgumentNullException(nameof(minecraftVersion));
            
            if (IsModListFileExist())
            {
                throw new MinecraftModUpdaterException($"A file {FILE_NAME} already exist.");
            }

            var modListFile = new ModListFile()
            {
                FileVersion = 1,
                MinecraftVersion = minecraftVersion,
                Mods = new List<ModData>()
            };

            await using var createStream = File.Create(_path + FILE_NAME);
            await JsonSerializer.SerializeAsync(createStream, modListFile);
        }

        /// <inheritdoc/>
        public async Task<ModListFile> ReadMinecraftModUpdaterFileAsync()
        {
            if (!IsModListFileExist())
            {
                await CreateModListFileAsync();
            }

            await using var openStream = File.OpenRead(_path + FILE_NAME);
            return await JsonSerializer.DeserializeAsync<ModListFile>(openStream);
        }

        /// <inheritdoc/>
        public async Task EditMinecraftModUpdaterFileAsync(ModListFile modListFile)
        {
            _ = modListFile ?? throw new ArgumentNullException(nameof(modListFile));
            
            if (!IsModListFileExist())
            {
                return;
            }

            await using var openWriteStream = new FileStream(_path + FILE_NAME, FileMode.Truncate);
            await JsonSerializer.SerializeAsync(openWriteStream, modListFile);
        }

        /// <inheritdoc/>
        public async Task AddModToModUpdaterFile(ModData mod)
        {
            _ = mod ?? throw new ArgumentNullException(nameof(mod));
            
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

        /// <inheritdoc/>
        public async Task UpdateModInModUpdaterFile(ModData mod)
        {
            _ = mod ?? throw new ArgumentNullException(nameof(mod));
            
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

        /// <inheritdoc/>
        public async Task RemoveModInModUpdaterFile(ModData mod)
        {
            _ = mod ?? throw new ArgumentNullException(nameof(mod));
            
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
            return File.Exists(_path + FILE_NAME);
        }
    }
}