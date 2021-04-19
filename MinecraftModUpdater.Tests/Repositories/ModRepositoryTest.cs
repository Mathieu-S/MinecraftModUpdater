using System;
using System.Threading.Tasks;
using MinecraftModUpdater.Core.Exceptions;
using MinecraftModUpdater.Core.Models.Curse;
using MinecraftModUpdater.Core.Repositories;
using Xunit;

namespace MinecraftModUpdater.Tests.Repositories
{
    public class ModRepositoryTest
    {
        [Fact]
        public async void Get_Mod_List()
        {
            // Act
            var result = await ModRepository.GetModsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async void Get_Mod()
        {
            // Act
            var result = await ModRepository.GetModAsync(238222);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Just Enough Items (JEI)", result.Name);
        }

        [Fact]
        public async Task Get_Mod_By_Invalid_Id()
        {
            // Act & Assert
            await Assert.ThrowsAsync<CurseApiException>(() => ModRepository.GetModAsync(1));
        }
        
        [Fact]
        public async Task Get_Mod_By_Invalid_Number_Id()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => ModRepository.GetModAsync(0));
        }
        
        [Fact]
        public async void Search_With_Name()
        {
            // Act
            var result = await ModRepository.SearchModByNameAsync("JEI");

            // Assert
            Assert.NotNull(result);
        }
        
        [Fact]
        public async Task Search_Without_Name()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => ModRepository.SearchModByNameAsync(null));
        }
        
        
        
        [Fact]
        public async void Get_ModFiles()
        {
            // Act
            var result = await ModRepository.GetModFilesAsync(238222);

            // Assert
            Assert.NotNull(result);
        }
        
        [Fact]
        public async void Get_ModFiles_Without_Id()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => ModRepository.GetModFilesAsync(0));
        }

        [Fact]
        public async void Download_ModFile()
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
        
        [Fact]
        public async void Download_ModFile_Without_Arg()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => ModRepository.GetStreamModFileAsync(null));
        }
    }
}
