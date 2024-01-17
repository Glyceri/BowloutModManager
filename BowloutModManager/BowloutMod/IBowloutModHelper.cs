using BowloutModManager.BowloutMod.Interfaces;
using Newtonsoft.Json;
using System;

namespace BowloutModManager.BowloutMod
{
    public static class IBowloutModHelper
    {
        public static T GetConfiguration<T>(this IBowloutMod bowloutMod) where T : IBowloutConfiguration
        {
            string ModPath = ModDirectoryHandler.ModConfigFolder + bowloutMod.Name + ".json";
            if (!ModDirectoryHandler.Instance.FindOrCreateFile(ModPath))
            {
                return default;
            }
            
            try
            {
                T jsonObject = JsonConvert.DeserializeObject<T>(ModDirectoryHandler.Instance.ReadFile(ModPath));
                SaveConfiguration(bowloutMod, jsonObject);
                return jsonObject;
            }catch(Exception e)
            {
                BLogger.WriteLineToLog("JSON ERROR: " + e.Message);
                return default;
            }
        }

        public static void SaveConfiguration(this IBowloutMod bowloutMod, IBowloutConfiguration bowloutConfig)
        {
            string ModPath = ModDirectoryHandler.ModConfigFolder + bowloutMod.Name + ".json";
            try
            {
                string json = JsonConvert.SerializeObject(bowloutConfig);
                ModDirectoryHandler.Instance.WriteFile(ModPath, json);
            }
            catch(Exception e)
            {
                BLogger.WriteTextToLog("Saving went wrong for: " + bowloutMod.Name + ", " + e.Message);
            }
        }
    }
}
