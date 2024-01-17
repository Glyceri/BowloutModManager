using BowloutModManager.BowloutMod;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;

namespace BowloutModManager
{
    public class ModLoader
    {
        IBowloutMod[] installedMods = new IBowloutMod[0];

        /// <summary>
        /// Gets all loaded and installed IBowloutMods.
        /// </summary>
        /// <remarks>
        /// Never cache these.
        /// </remarks>
        public IBowloutMod[] BowloutMods
        {
            get => installedMods;
        }

        public void AttemptToLoadMods()
        {
            DisposeOfOldMods();

            BLogger.WriteLineToLog("Attempting to load DLLs");

            string[] modFiles = GetAttemptedFiles();
            if (modFiles.Length == 0)
            {
                throw new NoModsFoundException();
            }

            IBowloutMod[] allBowloutMods = GatherValidMods(modFiles);
            if (allBowloutMods.Length == 0)
            {
                throw new NoModsLoadedException();
            }

            installedMods = allBowloutMods;

            BLogger.WriteLineToLog("Loaded the following mods: ");
            foreach (IBowloutMod mod in installedMods)
            {
                BLogger.WriteLineToLog($"[{mod.Name}] [{mod.Version?.ToString()}]");
            }

            SafeInvokeSetup();
        }

        void SafeInvokeSetup()
        {
            foreach (IBowloutMod mod in installedMods)
            {
                if (mod == null) continue;
                try
                {
                    mod.OnSetup();
                }
                catch(Exception ex)
                {
                    BLogger.WriteLineToLog("Failure in Setup: " + mod.Name + ", " + ex.Message);
                }
            }
        }

        public void DisposeOfOldMods()
        {
            BLogger.WriteLineToLog("Disposing old mods.");

            foreach(IBowloutMod mod in installedMods)
            {
                if (mod == null) continue;
                mod.Dispose();
            }
        }

        string[] GetAttemptedFiles()
        {
            try
            {
                return Directory.GetFiles("Mods/");
            }
            catch (Exception e)
            {
                BLogger.WriteLineToLog("Fail at attempting to get files in the Mods/ folder: " + e.Message);
            }

            return new string[0];
        }

        IBowloutMod[] GatherValidMods(string[] modfiles)
        {
            List<IBowloutMod> validBowloutMods = new List<IBowloutMod>();
            foreach (string modfile in modfiles)
            {
                string fullFile = GetFullFile(modfile);
                if (fullFile == null || fullFile == string.Empty)
                {
                    BLogger.WriteLineToLog("Full File is Null!");
                    continue;
                }

                Assembly assembly = LoadAssemly(fullFile);
                if (assembly == null)
                {
                    BLogger.WriteLineToLog("Assembly is Null!");
                    continue;
                }

                Type validAssembly = GetValidType(ref assembly);
                if (validAssembly == null)
                {
                    BLogger.WriteLineToLog($"Assembly: {modfile} is not a valid Bowlout Mod");
                    continue;
                }

                IBowloutMod bowloutMod = CreateBowloutMod(validAssembly);
                if (bowloutMod == null)
                {
                    BLogger.WriteLineToLog("Bowlout mod is Null!");
                    continue;
                }

                validBowloutMods.Add(bowloutMod);
            }

            return validBowloutMods.ToArray();
        }

        string GetFullFile(string baseFile)
        {
            try
            {
                return Path.GetFullPath(baseFile);
            }
            catch (Exception e)
            {
                BLogger.WriteLineToLog($"{e.Message}");
            }

            return string.Empty;
        }

        Assembly LoadAssemly(string path)
        {
            try
            {
                return Assembly.LoadFile(path);
            }
            catch (Exception e)
            {
                BLogger.WriteLineToLog("Failure to read/load mod assembly: " + e.Message);
            }

            return null;
        }

        Type GetValidType(ref Assembly modAssembly)
        {
            foreach (Type t in GetAssemblyTypes(ref modAssembly))
            {
                Type[] interfaces = GetInterfaceTypes(t);
                bool isInterfaceType = InterfaceIsValid(interfaces);
                if (isInterfaceType) return t;
            }

            return null;
        }

        Type[] GetAssemblyTypes(ref Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }catch (Exception e)
            {
                BLogger.WriteLineToLog("Failed to grab assembly types: " + e.Message);
            }

            return new Type[0];
        }

        Type[] GetInterfaceTypes(Type baseType)
        {
            try
            {
                return baseType.GetInterfaces(true).ToArray();
            }catch(Exception e)
            {
                BLogger.WriteLineToLog("Failed to grab Interface Types: " + e.Message);
            }
            return new Type[0];
        }

        bool InterfaceIsValid(Type[] interfaceTypes)
        {
            foreach (Type interfaceType in interfaceTypes)
            {
                if (interfaceType != typeof(IBowloutMod)) continue;
                return true;
            }

            return false;
        }

        IBowloutMod CreateBowloutMod(Type fromType) 
        {
            try
            {
                return Activator.CreateInstance(fromType) as IBowloutMod;
            }catch(Exception e)
            {
                BLogger.WriteLineToLog("Failed to create IBowloutMod: " + e.Message);
            }

            return null;
        }
    }
}
