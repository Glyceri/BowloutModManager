using BowloutModManager.BowloutMod.Interfaces;
using BowloutModManager.BowloutModded.CustomTypes;
using System;
namespace BowloutModManager.BowloutModded.ModlistMod
{
    [Serializable]
    public class ModlistConfiguration : IBowloutConfiguration
    {
        public int Version { get; set; } = 1;

        public BowloutBoolList ModList;

        public ModlistConfiguration() 
        {
            if (ModList == null) ModList = new BowloutBoolList();
        }
    }
}
