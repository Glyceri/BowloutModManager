using BowloutModManager.BowloutMod;
using BowloutModManager.BowloutMod.Interfaces;
using BowloutModManager.BowloutModded.CustomTypes;
using System;
using System.Collections.Generic;

namespace BowloutModManager.BowloutModded.ModlistMod
{
    public class BowloutModlistMod : IBowloutMod
    {
        public string Name => "Bowlout Mod List";

        public Version Version => new Version(1, 0, 0, 0);

        public string Description => "Literally a modlist.";

        public IBowloutConfiguration Configuration { get; set; } = null;
        ModlistConfiguration Settings => (ModlistConfiguration)Configuration;

        public bool Enabled { get; set; }

        public void OnSetup()
        {
            Configuration = this.GetConfiguration<ModlistConfiguration>() ?? new ModlistConfiguration();

            List<BowloutBoolValue> bowloutValues = new List<BowloutBoolValue>();
          
            foreach(IBowloutMod mod in Main.Instance.BowloutMods)
            {
                if (mod == null) continue;
                bool value = true;
                if (Settings.ModList.Contains(mod.Name)) value = Settings.ModList.Get(mod.Name);
                bowloutValues.Add(new BowloutBoolValue(value, mod.Name));
            }

            Settings.ModList.Clear();
            Settings.ModList.Set(bowloutValues);
            Settings.ModList.onChangeMod += (string mod, bool value) =>
            {
                foreach (IBowloutMod imod in Main.Instance.BowloutMods)
                {
                    if (imod.Name == mod) onModToggle?.Invoke(imod, value);
                }
            };

            for(int i = 0; i < Settings.ModList.Length; i++)
            {
                BowloutBoolValue value = Settings.ModList[i];
                onModToggle?.Invoke(GetMod(value.name), value.value);
            }

            this.SaveConfiguration(Configuration);
        }

        IBowloutMod GetMod(string name)
        {
            foreach(IBowloutMod mod in Main.Instance.BowloutMods)
            {
                if (mod.Name == name) return mod;
            }
            return null;
        }

        public delegate void OnModToggle(IBowloutMod mod, bool value);

        public event OnModToggle onModToggle;

        public void Dispose()
        {
           
        }

        public void OnDisable()
        {
            
        }

        public void OnEnable()
        {
            
        }

        public void OnFixedUpdate()
        {
           
        }

        public void OnLateUpdate()
        {

        }

        public void OnUpdate()
        {
            
        }

        public void OnGUI()
        {
            
        }
    }
}
