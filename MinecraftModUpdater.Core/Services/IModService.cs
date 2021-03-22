using System.Collections.Generic;
using System.Threading.Tasks;
using MinecraftModUpdater.Core.Models.Curse;

namespace MinecraftModUpdater.Core.Services
{
    public interface IModService
    {
        Task<CurseMod> SearchByIdAsync(uint modId);
        Task<IEnumerable<CurseMod>> SearchByNameAsync(string name, bool strictSearch = true);
        Task<CurseModFile> GetLastCompatibleRelease(uint modId, string minecraftVersion);
        Task<CurseModFile> GetSpecificRelease(uint modId, uint fileId);
        Task<bool> DownloadModFileAsync(CurseModFile mod);
        void DeleteModFile(string fileName);
        uint ConvertModId(string modId);
    }
}