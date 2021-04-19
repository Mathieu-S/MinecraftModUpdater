using System;
using System.Collections.Generic;

namespace MinecraftModUpdater.Core.Models.Curse
{
    /// <summary>
    /// Represents a mod in the Curse API.
    /// </summary>
    public class CurseMod
    {
        public uint Id { get; set; }
        public string Name  { get; set; }

        public IEnumerable<Author> Authors { get; set; }
        public IEnumerable<Attachment> Attachments { get; set; }

        public Uri WebsiteUrl { get; set; }
        public ushort GameId { get; set; }
        public string Summary { get; set; }
        public uint DefaultFileId { get; set; }
        public float DownloadCount { get; set; } // A float for download count ??? Really Curse ????!!!!

        //public IEnumerable<string> LatestFiles { get; set; }
        //public IEnumerable<string> Categories { get; set; }

        public byte Status { get; set; }
        public ushort PrimaryCategoryId { get; set; }

        //public IEnumerable<string> CategorySection { get; set; }

        public string Slug { get; set; }

        //public IEnumerable<string> GameVersionLatestFiles { get; set; }

        public bool IsFeatured { get; set; }
        public float PopularityScore { get; set; }
        public uint GamePopularityRank { get; set; }
        public string PrimaryLanguage { get; set; }
        public string GameSlug { get; set; }
        public string GameName { get; set; }
        public string PortalName { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateReleased { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsExperiemental { get; set; }
    }

    public class Author
    {
        public string Name { get; set; }
        public Uri Url { get; set; }
        public uint ProjectId { get; set; }
        public uint Id { get; set; }
        public string ProjectTitleTitle { get; set; }
        public uint UserId { get; set; }
        public uint? TwitchId { get; set; }
    }

    public class Attachment
    {
        public uint Id { get; set; }
        public uint ProjectId { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
        public Uri ThumbnailUrl { get; set; }
        public string Title { get; set; }
        public Uri Url { get; set; }
        public byte Status { get; set; }
    }
}