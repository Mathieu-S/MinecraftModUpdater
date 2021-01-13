using System;
using System.Collections.Generic;

namespace MinecraftModUpdater.Core.Models.Curse
{
    /// <summary>
    /// 
    /// </summary>
    public class CurseModFile
    {
        public uint Id { get; set; }
        public string DisplayName { get; set; }
        public string FileName { get; set; }
        public DateTime FileDate { get; set; }
        public uint FileLength { get; set; }
        public byte ReleaseType { get; set; }
        public byte FileStatus { get; set; }
        public Uri DownloadUrl { get; set; }
        public bool IsAlternate { get; set; }
        public byte AlternateFileId { get; set; }

        public IEnumerable<ModFileDependency> Dependencies { get; set; }

        public bool IsAvailable { get; set; }

        //public IEnumerable<string> Modules { get;set;}

        public uint PackageFingerprint { get; set; }

        public IEnumerable<string> GameVersion { get; set; }

        public string InstallMetadata { get; set; }
        public string ServerPackFileId { get; set; }
        public bool HasInstallScript { get; set; }
        public DateTime GameVersionDateReleased { get; set; }
        public string gameVersionFlavor { get; set; }
    }

    public class ModFileDependency
    {
        public uint AddonId { get; set; }
        public byte Type { get; set; }
    }
}
