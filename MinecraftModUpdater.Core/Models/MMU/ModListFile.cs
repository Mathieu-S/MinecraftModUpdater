using System.Collections.Generic;

namespace MinecraftModUpdater.Core.Models.MMU
{
    public class ModListFile
    {
        public byte FileVersion { get; set; }
        public string MinecraftVersion { get; set; }
        public IList<ModData> Mods { get; set; }
    }

    public class ModData
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public uint Version { get; set; }
    }
}
