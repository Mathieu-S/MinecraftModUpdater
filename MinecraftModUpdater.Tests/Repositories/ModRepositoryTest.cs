using System;
using MinecraftModUpdater.Core.Models.Curse;
using MinecraftModUpdater.Core.Repositories;
using Xunit;

namespace MinecraftModUpdater.Tests.Repositories
{
    public class ModRepositoryTest
    {
        [Fact]
        public async void GetModList()
        {
            // Act
            var result = await ModRepository.GetModsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async void GetMod()
        {
            // Act
            var result = await ModRepository.GetModAsync(238222);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Just Enough Items (JEI)", result.Name);
        }

        [Fact]
        public async void GetModFiles()
        {
            // Act
            var result = await ModRepository.GetModFilesAsync(238222);

            // Assert
            Assert.NotNull(result);
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
            var result = await ModRepository.GetStreamModFileAsync(mod);

            // Assert
            Assert.NotNull(result);
        }
    }
}
