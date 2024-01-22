using BowloutModManager.BowloutMod;
using System;
using UnityEngine;

namespace BowloutModManager
{
    public class BowloutTicker : MonoBehaviour
    {
        void Update ()
        {
            foreach(IBowloutMod mod in Main.Instance.BowloutMods)
            {
                if (mod == null) continue;
                if (!mod.Enabled) continue;
                try
                {
                    mod?.OnUpdate();
                }
                catch(Exception e)
                {
                    BLogger.WriteLineToLog($"Error in update for mod: {mod.Name} with error: {e}");
                }
            }
        }

        void FixedUpdate()
        {
            foreach (IBowloutMod mod in Main.Instance.BowloutMods)
            {
                if (mod == null) continue;
                if (!mod.Enabled) continue;
                try
                {
                    mod?.OnFixedUpdate();
                }
                catch (Exception e)
                {
                    BLogger.WriteLineToLog($"Error in fixed update for mod: {mod.Name} with error: {e}");
                }
            }
        }

        void LateUpdate()
        {
            foreach (IBowloutMod mod in Main.Instance.BowloutMods)
            {
                if (mod == null) continue;
                if (!mod.Enabled) continue;
                try
                {
                    mod?.OnLateUpdate();
                }
                catch (Exception e)
                {
                    BLogger.WriteLineToLog($"Error in late update for mod: {mod.Name} with error: {e}");
                }
            }
        }

        void OnGUI()
        {
            foreach (IBowloutMod mod in Main.Instance.BowloutMods)
            {
                if (mod == null) continue;
                if (!mod.Enabled) continue;
                try
                {
                    mod?.OnGUI();
                }
                catch (Exception e)
                {
                    BLogger.WriteLineToLog($"Error in OnGUI for mod: {mod.Name} with error: {e}");
                }
            }
        }
    }
}
