using System;
using System.IO;

namespace BowloutModManager
{
    public static class BLogger
    {
        public const string ModlogPath = "ModLog/log.txt";

        static BLogger()
        {
#if DEBUG
            CreateModLogDirectory();
            CreateModLogFile();
#endif
        }

        static void CreateModLogDirectory()
        {
            try
            {
                Directory.CreateDirectory("ModLog");
            }
            catch (Exception e)
            {
                WriteLineToLog(e.Message);
            }
        }

        static void CreateModLogFile()
        {
            try
            {
                File.Copy(ModlogPath, "ModLog/log_old.txt", true);
            }
            catch(Exception e) 
            {
                WriteLineToLog(e.Message);
            }

            try
            {
                File.Delete(ModlogPath);
            }
            catch (Exception e)
            {
                WriteLineToLog(e.Message);
            }

            try
            {
                File.CreateText(ModlogPath).Close();
            }
            catch (Exception e) 
            {
                WriteLineToLog(e.Message);
            }
        }

        public static void WriteLineToLog(string line) => WriteTextToLog(line + '\n');
        public static void WriteTextToLog(string logText)
        {
            try
            {
                File.AppendAllText(ModlogPath, logText);
            }
            catch { }
        }


        public static void DebugAllMods()
        {
            string[] files = Directory.GetFiles("Mods/");
            foreach (string file in files)
                WriteLineToLog(file);
        }
    }
}
