using BowloutModManager.BowloutMod;
using BowloutModManager.BowloutModded.Interfaces;
using System;

namespace BowloutModManager
{
    public class Main : SingletonBase<Main>
    {
        ModLoader bowloutModLoader = new ModLoader();
        BowloutSceneManager bowloutSceneManager = new BowloutSceneManager();
        ModDirectoryHandler modDirectoryHandler = new ModDirectoryHandler();

        public IBowloutMod[] BowloutMods => bowloutModLoader.BowloutMods;

        void InternalStart()
        {
            modDirectoryHandler.HandleDirectories();

            if (!LoadAllMods())
            {
#if !DEBUG
                BLogger.WriteLineToLog("Failed to load any mods. There is no reason for the mod loader to stay active. Deactivating NOW!");
                return;
#endif
            }
        }

        bool LoadAllMods()
        {
            try 
            {
                bowloutModLoader.AttemptToLoadMods();
                return true;
            }
            catch(Exception e)
            {
                BLogger.WriteTextToLog(e.Message);
                return false;
            }
        }

        public static void Start() => new Main().InternalStart();
    }
}
