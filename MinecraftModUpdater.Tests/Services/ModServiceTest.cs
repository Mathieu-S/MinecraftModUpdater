using System;
using MinecraftModUpdater.Core.Models.Curse;
using MinecraftModUpdater.Core.Services;
using Xunit;

namespace MinecraftModUpdater.Tests.Services
{
    public class ModServiceTest
    {
        private readonly ModService _modService;

        public ModServiceTest()
        {
            _modService = new ModService(@"E:\Bureau\Sandbox\");
        }

        [Fact]
        public async void DownloadModFile()
        {
            // Arrange
            var mod = new CurseModFile
            {
                Id = 2344374,
                FileName = "jei_1.10.2-3.13.3.369.jar",
                DownloadUrl = new Uri("https://edge.forgecdn.net/files/2344/374/jei_1.10.2-3.13.3.369.jar")
            };

            // Act
            var result = await _modService.DownloadModFileAsync(mod);

            // Assert
            Assert.True(result);
        }
    }
}
