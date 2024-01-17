using BowloutModManager.BowloutModded.Interfaces;
using System;
using System.IO;

namespace BowloutModManager
{
    public class ModDirectoryHandler : SingletonBase<ModDirectoryHandler>
    {
        public const string ModsFolder = "Mods/";
        public const string ModConfigFolder = ModsFolder + "ModConfig/";

        public void HandleDirectories()
        {
            if (!FindOrCreateDirectory("Mods/"))
            {
                BLogger.WriteLineToLog("Error in Mods/");
                return;
            }

            if (!FindOrCreateDirectory("Mods/ModConfig"))
            {
                BLogger.WriteLineToLog("Error in Mods/ModConfig");
                return;
            }
        }

        public bool FindOrCreateDirectory(string path)
        {
            if (Directory.Exists(path)) return true;
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
                BLogger.WriteLineToLog("Failed to create directory: " + e.Message);
                return false;
            }
            return Directory.Exists(path);
        }

        public bool FindOrCreateFile(string path)
        {
            if (File.Exists(path)) return true;
            try
            {
                File.Create(path).Close();
            }
            catch (Exception e)
            {
                BLogger.WriteLineToLog("Failed to create file: " + e.Message);
                return false;
            }
            return File.Exists(path);
        }

        public string ReadFile(string path)
        {
            if (!File.Exists(path)) return string.Empty;
            try
            {
                return File.ReadAllText(path);
            }
            catch(Exception e)
            {
                BLogger.WriteLineToLog(e.Message);
                return string.Empty;
            }
        }

        public void WriteFile(string path, string data)
        {
            if (!FindOrCreateFile(path)) return;

            try
            {
                File.WriteAllText(path, data);
            }catch(Exception e)
            {
                BLogger.WriteLineToLog("Writing file failed: " + e.Message);
            }
        }
    }
}
