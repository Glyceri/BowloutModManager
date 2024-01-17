using BowloutModManager.BowloutMod;
using BowloutModManager.BowloutMod.Interfaces;
using System;

namespace BowloutModManager.BowloutModded.ModlistMod
{
    public class BowloutModlist : IBowloutMod
    {
        public string Name => "Bowlout Mod List";

        public Version Version => new Version(1, 0, 0, 0);

        public string Description => "Literally a modlist.";

        public IBowloutConfiguration Configuration => throw new NotImplementedException();

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

        public void OnSetup()
        {
            
        }

        public void OnUpdate()
        {
            
        }
    }
}
