using System;
using System.Collections.Generic;

namespace MinecraftModUpdater.Core.Models.MMU
{
    public class ModListFile
    {
        public byte FileVersion { get; set; }
        public string MinecraftVersion { get; set; }
        public IList<ModData> Mods { get; set; }
    }

    public class ModData : IEquatable<ModData>
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public uint Version { get; set; }
        public bool Equals(ModData other)
        {
            return Id == other.Id && Version == other.Version;
        }
    }
}
