using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using MinecraftModUpdater.Core.Exceptions;
using MinecraftModUpdater.Core.Models.Curse;

namespace MinecraftModUpdater.Core.Repositories
{
    /// <summary>
    /// A class for accessing the Curse API.
    /// </summary>
    public static class ModRepository
    {
        /// <summary>
        /// The base URL
        /// </summary>
        private const string BASE_URL = "https://addons-ecs.forgesvc.net/api/v2/addon";

        /// <summary>
        /// Gets the list of mods from the Cruse API asynchronously.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="CurseApiException">
        /// Minecraft Mod Updater cannot access API, please check your internet connection.
        /// or
        /// Minecraft Mod Updater cannot parse the API. This happens if Curse change their structure. Please, open an issue.
        /// </exception>
        public static async Task<IEnumerable<CurseMod>> GetModsAsync()
        {
            using var client = new HttpClient();
            try
            {
                return await client.GetFromJsonAsync<IEnumerable<CurseMod>>($"{BASE_URL}/search?gameId=432&sectionId=6");
            }
            catch (HttpRequestException ex)
            {
                throw new CurseApiException("Minecraft Mod Updater cannot access API, please check your internet connection.", ex.InnerException);
            }
            catch (JsonException ex)
            {
                throw new CurseApiException("Minecraft Mod Updater cannot parse the API. This happens if Curse change their structure. Please, open an issue.", ex.InnerException);
            }
        }

        /// <summary>
        /// Gets a mod from the Cruse API asynchronously.
        /// </summary>
        /// <param name="modId">The mod identifier.</param>
        /// <returns></returns>
        /// <exception cref="CurseApiException">
        /// Minecraft Mod Updater cannot access API, please check your internet connection.
        /// or
        /// Minecraft Mod Updater cannot parse the API. This happens if Curse change their structure. Please, open an issue.
        /// </exception>
        public static async Task<CurseMod> GetModAsync(uint modId)
        {
            _ = modId == 0 ? throw new ArgumentException("modId can't be 0.", nameof(modId)) : 0;
            
            using var client = new HttpClient();
            try
            {
                return await client.GetFromJsonAsync<CurseMod>($"{BASE_URL}/{modId}");
            }
            catch (HttpRequestException ex)
            {
                throw new CurseApiException("Minecraft Mod Updater cannot access API, please check your internet connection.", ex.InnerException);
            }
            catch (JsonException ex)
            {
                throw new CurseApiException("Minecraft Mod Updater cannot parse the API. This happens if Curse change their structure. Please, open an issue.", ex.InnerException);
            }
        }

        /// <summary>
        /// Searches the mod by name asynchronously.
        /// </summary>
        /// <param name="modName">Name of the mod.</param>
        /// <returns></returns>
        /// <exception cref="CurseApiException">
        /// Minecraft Mod Updater cannot access API, please check your internet connection.
        /// or
        /// Minecraft Mod Updater cannot parse the API. This happens if Curse change their structure. Please, open an issue.
        /// </exception>
        public static async Task<IEnumerable<CurseMod>> SearchModByNameAsync(string modName)
        {
            _ = modName ?? throw new ArgumentNullException(nameof(modName));
            
            using var client = new HttpClient();
            try
            {
                return await client.GetFromJsonAsync<IEnumerable<CurseMod>>($"{BASE_URL}/search?gameId=432&sectionId=6&searchFilter={modName}");
            }
            catch (HttpRequestException ex)
            {
                throw new CurseApiException("Minecraft Mod Updater cannot access API, please check your internet connection.", ex.InnerException);
            }
            catch (JsonException ex)
            {
                throw new CurseApiException("Minecraft Mod Updater cannot parse the API. This happens if Curse change their structure. Please, open an issue.", ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the mod files asynchronous.
        /// </summary>
        /// <param name="modId">The mod identifier.</param>
        /// <returns></returns>
        /// <exception cref="CurseApiException">
        /// Minecraft Mod Updater cannot access API, please check your internet connection.
        /// or
        /// Minecraft Mod Updater cannot parse the API. This happens if Curse change their structure. Please, open an issue.
        /// </exception>
        public static async Task<IEnumerable<CurseModFile>> GetModFilesAsync(uint modId)
        {
            _ = modId == 0 ? throw new ArgumentException("modId can't be 0.", nameof(modId)) : 0;
            
            using var client = new HttpClient();
            try
            {
                return await client.GetFromJsonAsync<IEnumerable<CurseModFile>>($"{BASE_URL}/{modId}/files");
            }
            catch (HttpRequestException ex)
            {
                throw new CurseApiException("Minecraft Mod Updater cannot access API, please check your internet connection.", ex.InnerException);
            }
            catch (JsonException ex)
            {
                throw new CurseApiException("Minecraft Mod Updater cannot parse the API. This happens if Curse change their structure. Please, open an issue.", ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the stream mod file asynchronous.
        /// </summary>
        /// <param name="mod">The mod.</param>
        /// <returns></returns>
        /// <exception cref="CurseApiException">Minecraft Mod Updater cannot access API, please check your internet connection.</exception>
        public static async Task<Stream> GetStreamModFileAsync(CurseModFile mod)
        {
            _ = mod ?? throw new ArgumentNullException(nameof(mod));
            
            using var client = new HttpClient();
            try
            {
                return await client.GetStreamAsync(mod.DownloadUrl);
            }
            catch (HttpRequestException ex)
            {
                throw new CurseApiException("Minecraft Mod Updater cannot access API, please check your internet connection.", ex.InnerException);
            }
        }
    }
}
