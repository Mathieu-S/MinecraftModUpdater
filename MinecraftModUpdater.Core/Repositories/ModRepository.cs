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
        private const string BASE_URL = "https://addons-ecs.forgesvc.net/api/v2/addon/";

        /// <summary>
        /// Gets the mods asynchronous.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="MinecraftModUpdater.Core.Exceptions.CurseApiException">
        /// Curse Mod Updater cannot access API, please check your internet connection.
        /// -or-
        /// Curse Mod Updater cannot parse the API. This happens if Curse change their structure. Please, open an issue.
        /// </exception>
        public static async Task<IEnumerable<CurseMod>> GetModsAsync()
        {
            using var client = new HttpClient();
            try
            {
                return await client.GetFromJsonAsync<IEnumerable<CurseMod>>(BASE_URL + "search?gameId=432&sectionId=6");
            }
            catch (HttpRequestException ex)
            {
                throw new CurseApiException("Curse Mod Updater cannot access API, please check your internet connection.", ex.InnerException);
            }
            catch (JsonException ex)
            {
                throw new CurseApiException("Curse Mod Updater cannot parse the API. This happens if Curse change their structure. Please, open an issue.", ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the mod asynchronous.
        /// </summary>
        /// <param name="modId">The mod identifier.</param>
        /// <returns></returns>
        /// <exception cref="MinecraftModUpdater.Core.Exceptions.CurseApiException">
        /// Curse Mod Updater cannot access API, please check your internet connection.
        /// -or-
        /// Curse Mod Updater cannot parse the API. This happens if Curse change their structure. Please, open an issue.
        /// </exception>
        public static async Task<CurseMod> GetModAsync(uint modId)
        {
            using var client = new HttpClient();
            try
            {
                return await client.GetFromJsonAsync<CurseMod>(BASE_URL + modId);
            }
            catch (HttpRequestException ex)
            {
                throw new CurseApiException("Curse Mod Updater cannot access API, please check your internet connection.", ex.InnerException);
            }
            catch (JsonException ex)
            {
                throw new CurseApiException("Curse Mod Updater cannot parse the API. This happens if Curse change their structure. Please, open an issue.", ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the mod files asynchronous.
        /// </summary>
        /// <param name="modId">The mod identifier.</param>
        /// <returns></returns>
        /// <exception cref="MinecraftModUpdater.Core.Exceptions.CurseApiException">
        /// Curse Mod Updater cannot access API, please check your internet connection.
        /// -or-
        /// Curse Mod Updater cannot parse the API. This happens if Curse change their structure. Please, open an issue.
        /// </exception>
        public static async Task<IEnumerable<CurseModFile>> GetModFilesAsync(uint modId)
        {
            using var client = new HttpClient();
            try
            {
                return await client.GetFromJsonAsync<IEnumerable<CurseModFile>>(BASE_URL + modId + "/files");
            }
            catch (HttpRequestException ex)
            {
                throw new CurseApiException("Curse Mod Updater cannot access API, please check your internet connection.", ex.InnerException);
            }
            catch (JsonException ex)
            {
                throw new CurseApiException("Curse Mod Updater cannot parse the API. This happens if Curse change their structure. Please, open an issue.", ex.InnerException);
            }
        }

        /// <summary>
        /// Gets the stream mod file asynchronous.
        /// </summary>
        /// <param name="mod">The mod.</param>
        /// <returns></returns>
        /// <exception cref="MinecraftModUpdater.Core.Exceptions.CurseApiException">
        /// Curse Mod Updater cannot access API, please check your internet connection.
        /// -or-
        /// Curse Mod Updater cannot parse the API. This happens if Curse change their structure. Please, open an issue.
        /// </exception>
        public static async Task<Stream> GetStreamModFileAsync(CurseModFile mod)
        {
            using var client = new HttpClient();
            try
            {
                return await client.GetStreamAsync(mod.DownloadUrl);
            }
            catch (HttpRequestException ex)
            {
                throw new CurseApiException("Curse Mod Updater cannot access API, please check your internet connection.", ex.InnerException);
            }
        }
    }
}
