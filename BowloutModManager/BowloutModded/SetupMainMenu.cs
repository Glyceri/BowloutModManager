using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace BowloutModManager.BowloutModded
{
    public class SetupMainMenu : MonoBehaviour
    {
        void Start()
        {
            BLogger.WriteLineToLog("START!");
            try
            {
                GameObject quitGameButton = GameObject.Find("B_Settings_Cogwheel");
                if (quitGameButton == null)
                {
                    BLogger.WriteLineToLog("Quitter is null!");
                    return;
                }

                GameObject copy = GameObject.Instantiate(quitGameButton, quitGameButton.transform.parent);
                RectTransform rectTransform = copy.GetComponent<RectTransform>();
                rectTransform.localPosition += new Vector3(-(rectTransform.rect.width), 0);
                Button b = copy.GetComponent<Button>();
                b.onClick.SetPersistentListenerState(1, UnityEngine.Events.UnityEventCallState.Off);
                b.onClick.SetPersistentListenerState(2, UnityEngine.Events.UnityEventCallState.Off);

                
                PauseSubmenu_Settings settingsMenu = GameObject.FindObjectOfType<PauseSubmenu_Settings>(true);
                if (settingsMenu == null)
                {
                    BLogger.WriteLineToLog("settingsMenu NULL!");
                    return;
                }
                GameObject pauseMenu = settingsMenu.gameObject;
                if (pauseMenu == null)
                {
                    BLogger.WriteLineToLog("Pause Menu NULL!");
                    return;
                }

                GameObject pauseClone = GameObject.Instantiate(pauseMenu, pauseMenu.transform.parent);
                pauseClone.name = "ModMenu";

                GameObject.Destroy(pauseClone.GetComponent<PauseSubmenu_Settings>());
                PauseSubmenu_ModSettings modSettings = pauseClone.AddComponent<PauseSubmenu_ModSettings>();
                PauseHandler pauseHandler = GameObject.FindObjectOfType<PauseHandler>(true);

                FieldInfo info = pauseHandler.GetType().GetField("submenus", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (info != null)
                {
                    Pause_Submenu[] submenus = (Pause_Submenu[])info.GetValue(pauseHandler);
                    List<Pause_Submenu> newSubmenus = submenus.ToList();
                    newSubmenus.Add(modSettings);
                    info.SetValue(pauseHandler, newSubmenus.ToArray());
                }
                else
                {
                    BLogger.WriteLineToLog("Couldnt get 'submenus' field!");
                }

                b.onClick.AddListener(() => 
                {
                    if (pauseHandler == null)
                    {
                        BLogger.WriteLineToLog("Pause Handler NULL!");
                        return;
                    }
                    pauseHandler.GAME_PAUSE();
                    pauseHandler.OpenSubmenu(modSettings);
                });
                GameObject.Destroy(pauseClone.transform.GetChild(1).gameObject);
                
                Transform allSettingsObject = pauseClone.transform.GetChild(0);
                if (allSettingsObject == null)
                {
                    BLogger.WriteLineToLog("allSettingsObject NULL!");
                    return;
                }

                int childCount = allSettingsObject.childCount;

                List<SettingsColumnModded> settings = new List<SettingsColumnModded>();

                for (int i = childCount - 1; i >= 0; i--)
                {
                    BLogger.WriteLineToLog("CHILD!");
                    Transform column = allSettingsObject.GetChild(i);
                    settings.Add(column.gameObject.AddComponent<SettingsColumnModded>());

                }

                foreach(SettingsColumnModded settingsColumnModded in settings)
                {
                    settingsColumnModded.Init();
                    modSettings.RegisterColumn(settingsColumnModded);
                }

               
            }
            catch (Exception e)
            {
                BLogger.WriteLineToLog($"Error: {e.Message} - {e.Source} - {e.StackTrace} - {e.TargetSite}");
            }
        }
    }
}
