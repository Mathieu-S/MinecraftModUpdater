using System.Collections.Generic;
using MinecraftModUpdater.Core.Models.MMU;
using MinecraftModUpdater.Core.Services;
using Xunit;

namespace MinecraftModUpdater.Tests.Services
{
    public class ModListFileServiceTest
    {
        private readonly ModListFileService _modListFileService;

        public ModListFileServiceTest()
        {
            _modListFileService = new ModListFileService(@"E:\Bureau\Sandbox\");
        }
        
        [Fact]
        public async void CreateFile()
        {
            // Act
            await _modListFileService.CreateModListFileAsync();
        }
        
        [Fact]
        public async void ReadFile()
        {
            // Act
            var result = await _modListFileService.ReadMinecraftModUpdaterFileAsync();
        
            // Assert
            Assert.NotNull(result);
        }
        
        [Fact]
        public async void EditFile()
        {
            // Arrange
            var file = new ModListFile()
            {
                FileVersion = 1,
                MinecraftVersion = "1.16.4",
                Mods = new List<ModData>()
            };
        
            // Act
             await _modListFileService.EditMinecraftModUpdaterFileAsync(file);
        }
    }
}