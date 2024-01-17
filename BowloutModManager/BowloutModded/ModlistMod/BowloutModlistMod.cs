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

        public IBowloutConfiguration Configuration => this.GetConfiguration<ModlistConfiguration>() ?? new ModlistConfiguration();
        ModlistConfiguration Settings => (ModlistConfiguration)Configuration;

        public void OnSetup()
        {
            IBowloutMod[] activeMods = Main.Instance.BowloutMods;

            List<BowloutBoolValue> bowloutValues = new List<BowloutBoolValue>();
          
            foreach(IBowloutMod mod in activeMods)
            {
                bool value = true;
                if (Settings.ModList.Contains(mod.Name)) value = Settings.ModList.Get(mod.Name);
                bowloutValues.Add(new BowloutBoolValue(value, mod.Name));
            }

            Settings.ModList.Clear();
            Settings.ModList.Set(bowloutValues);
            this.SaveConfiguration(Settings);
        }

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
    }
}
