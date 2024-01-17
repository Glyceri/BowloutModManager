using BowloutModManager.BowloutMod;
using BowloutModManager.BowloutMod.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BowloutModManager.BowloutModded
{
    public class PauseSubmenu_ModSettings : Pause_Submenu
    {
        readonly List<SettingsColumnModded> columns = new List<SettingsColumnModded>();

        int atMod = 0;

        public void RegisterColumn(SettingsColumnModded column)
        {
            columns.Add(column);
        }

        void Awake()
        {
            onSubmenuOpened?.AddListener(OnOpen);
            onSubmenuClosed?.AddListener(OnClose);
            OnOpen();
        }

        void OnDestroy()
        {
            onSubmenuOpened?.RemoveListener(OnOpen);
            onSubmenuClosed?.RemoveListener(OnClose);
            OnClose();
        }

        void OnOpen()
        {
            Redraw();
        }

        void OnClose()
        {
            //columns[0].Cleanup();
        }

        IBowloutMod[] currentMods;

        void Update()
        {
            bool shouldRedraw = false;
            
            if (Keyboard.current[Key.LeftArrow].wasPressedThisFrame) 
            { 
                atMod -= 1; 
                shouldRedraw = true; 
            }
            if (Keyboard.current[Key.RightArrow].wasPressedThisFrame)
            { 
                atMod += 1; 
                shouldRedraw = true;
            }

            if (atMod < 0) atMod = currentMods.Length - 1;
            if (atMod >= currentMods.Length) atMod = 0;

            if (shouldRedraw) Redraw();
        }

        List<int> blackenedInts = new List<int>();

        void Redraw()
        {
            blackenedInts.Clear();
            currentMods = Main.Instance.BowloutMods;
            int counter = atMod;
            for (int i = columns.Count - 1; i >= 0; i--)
            {
                columns[i].Cleanup();
                if (counter >= currentMods.Length || blackenedInts.Contains(counter))
                {
                    columns[i].SetName("");
                }
                else
                {
                    IBowloutMod mod = currentMods[counter];
                    columns[i].SetName(mod.Name);

                    IBowloutConfiguration config = mod.Configuration;

                    FieldInfo[] fields = config.GetType().GetFields();

                    foreach(FieldInfo field in fields)
                    {
                        string fieldName = field.Name;
                        fieldName = AddSpacesToSentence(fieldName, false);
                        object obj = field.GetValue(config);
                        if (obj is bool bValue)
                        {
                            columns[i].AddToggle(fieldName, bValue, (newVal) => 
                            {
                                field.SetValue(config, newVal);
                                mod.SaveConfiguration(config);
                            });
                        }else if (obj is int iValue)
                        {
                            RangeAttribute range = field.GetAttribute<RangeAttribute>(true);
                            if (range == null) continue;
                            columns[i].AddSlider(fieldName, iValue, (int)range.min, (int)range.max, (iVal) =>
                            {
                                field.SetValue(config, iVal);
                                mod.SaveConfiguration(config);
                            });
                        }
                        else if (obj is float fValue)
                        {
                            RangeAttribute range = field.GetAttribute<RangeAttribute>(true);
                            if (range == null) continue;
                            columns[i].AddSlider(fieldName, fValue, range.min, range.max, (iVal) =>
                            {
                                field.SetValue(config, iVal);
                                mod.SaveConfiguration(config);
                            });
                        }
                    }

                }
                blackenedInts.Add(counter);
                counter++;
                if (counter >= currentMods.Length) counter = 0;
            }
        }

        string AddSpacesToSentence(string text, bool preserveAcronyms)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }
    }
}
