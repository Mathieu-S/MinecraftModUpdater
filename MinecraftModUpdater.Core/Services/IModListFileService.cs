using System.Threading.Tasks;
using MinecraftModUpdater.Core.Models.MMU;

namespace MinecraftModUpdater.Core.Services
{
    public interface IModListFileService
    {
        Task CreateModListFileAsync();
        Task CreateModListFileAsync(string minecraftVersion);
        Task<ModListFile> ReadMinecraftModUpdaterFileAsync();
        Task EditMinecraftModUpdaterFileAsync(ModListFile data);
        Task AddModToModUpdaterFile(ModData mod);
        Task UpdateModInModUpdaterFile(ModData mod);
        Task RemoveModInModUpdaterFile(ModData mod);
    }
}