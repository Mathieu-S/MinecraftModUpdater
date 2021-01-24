using System;
using System.Linq;
using MinecraftModUpdater.Core.Models.Curse;
using MinecraftModUpdater.Core.Repositories;
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
        public async void SearchByName_OneTerm()
        {
            // Arrange
            const string terms = "jei";
            var modFromApi = await ModRepository.SearchModByNameAsync(terms);

            // Act
            var result = await _modService.SearchByNameAsync(terms);

            // Assert
            Assert.True(modFromApi.Count() == result.Count());
        }
        
        [Fact]
        public async void SearchByName_TwoTerms()
        {
            // Arrange
            const string terms = "jei thaumic";
            var modFromApi = await ModRepository.SearchModByNameAsync(terms);

            // Act
            var result = await _modService.SearchByNameAsync(terms);

            // Assert
            Assert.False(modFromApi.Count() == result.Count());
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
