using Lean.Localization;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BowloutModManager.BowloutModded
{
    public class SettingsColumnModded : ModdedSettingBase
    {
        public RectTransform scrollPanel;
        public ScrollRect scrollRect;

        protected override void OnStart()
        {
            BLogger.WriteLineToLog("START! COLUMN");
            base.OnStart();
            FindScrollrect();
            HelpFind();
            Cleanup();
        }

        void FindScrollrect()
        {
            scrollRect = GetComponent<ScrollRect>();
            scrollPanel = transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();
        }

        public void Cleanup()
        {
            int childCount = scrollPanel.transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
                GameObject.Destroy(scrollPanel.transform.GetChild(i).gameObject);
            Resize();
        }

        public void AddSlider(string name, int min, int max, Action<int> callback) => AddSlider(name, min, max, (fValue) => callback?.Invoke((int)fValue), SliderType.INT);
        public void AddSlider(string name, float min, float max, Action<float> callback, SliderType sliderType = SliderType.FLOAT)
        {
            BLogger.WriteLineToLog("add SLIDER!");
            ModdedSliderSetting slider = Clone(SettingsColumnModdedHelper.SliderElement);
            slider.Cleanup(min, max, callback, sliderType);
            if (slider == null)
            {
                BLogger.WriteLineToLog("Slider NULL!");
                return;
            }
            try
            {
                slider.SetName(name);
            }catch(Exception e) { BLogger.WriteLineToLog($"Error: {e.Message}"); }
            Resize();
        }

        public void AddToggle(string name, bool startValue, Action<bool> onValueChange)
        {
            ModdedToggleSetting toggle = Clone(SettingsColumnModdedHelper.ToggleElement);
            if (toggle == null)
            {
                BLogger.WriteLineToLog("Toggle NULL!");
                return;
            }
            toggle.SetName(name);
            toggle.toggle.SetIsOnWithoutNotify(startValue);
            toggle.toggle.onValueChanged.AddListener((bVal) => onValueChange?.Invoke(bVal));
            Resize();
        }

        T Clone<T>(T fromObject) where T : ModdedSettingBase
        {
            if (fromObject == null)
            {
                BLogger.WriteLineToLog("From Object is NULL: " + typeof(T).Name);
                return null;
            }
            try
            {
                T t = GameObject.Instantiate(fromObject.gameObject).GetComponent<T>();
                //t.Init();
                Resize();
                t.transform.SetParent(scrollPanel.transform, false);
                return t;
            }catch(Exception e) { BLogger.WriteLineToLog(e.ToString()); }

            return null;
        }

        void Resize()
        {
            if(!scrollPanel.gameObject.TryGetComponent(out ContentSizeFitter sizeFitter)) sizeFitter = scrollPanel.gameObject.AddComponent<ContentSizeFitter>();
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollPanel);
        }

        // Settings Column has a Duty to help!
        void HelpFind()
        {
            int childCount = scrollPanel.transform.childCount;
            for(int i = 0; i < childCount; i++)
            {
                HandleAsSlider(scrollPanel.transform.GetChild(i));
                HandleAsToggle(scrollPanel.transform.GetChild(i));
            }
        }

        void HandleAsSlider(Transform baseChild)
        {
            if (baseChild == null)
            {
                return;
            }
            try
            {
                if (SettingsColumnModdedHelper.SliderElement != null)
                {
                    return;
                }
                if (!HasPotentialSlider(baseChild))
                {
                    return;
                }
                if (baseChild.GetChild(1).GetComponent<Slider>() == null)
                {
                    return;
                }
                BLogger.WriteLineToLog("FOUND SLIDER!");
                // Is Slider Setting!
                Transform slider = GameObject.Instantiate(baseChild);
                if (slider == null)
                {
                    return;
                }
                ModdedSliderSetting sliderSetting = slider.gameObject.AddComponent<ModdedSliderSetting>();
                sliderSetting.Cleanup(0, 0, (nothing) => { });
                SettingsColumnModdedHelper.SliderElement = sliderSetting;
            }
            catch(Exception e) { BLogger.WriteLineToLog(e.ToString()); }
        }

        void HandleAsToggle(Transform baseChild)
        {
            try
            {
                if (SettingsColumnModdedHelper.ToggleElement != null)
                {
                    return;
                }
                if (!HasPotentialToggle(baseChild))
                {
                    return;
                }
                if (baseChild.GetChild(1).GetComponent<Toggle>() == null)
                {
                    return;
                }
                BLogger.WriteLineToLog("FOUND TOGGLE!");
                // Is Toggle Setting!
                Transform toggle = GameObject.Instantiate(baseChild);
                if (toggle == null)
                {
                    return;
                }
                ModdedToggleSetting toggleSetting = toggle.gameObject.AddComponent<ModdedToggleSetting>();
                SettingsColumnModdedHelper.ToggleElement = toggleSetting;
            }
            catch (Exception e) { BLogger.WriteLineToLog(e.ToString()); }
        }

        bool HasPotentialSlider(Transform baseChild)
        {
            try
            {
                if (baseChild.childCount != 3) return false;
                if (baseChild.GetChild(0).GetComponent<TextMeshProUGUI>() == null) return false;
                if (baseChild.GetChild(2).GetComponent<TextMeshProUGUI>() == null) return false;
                return true;
            }
            catch(Exception e) 
            { 
                BLogger.WriteLineToLog(e.ToString()); 
            }

            return false;
        }

        bool HasPotentialToggle(Transform baseChild)
        {
            try
            {
                if (baseChild.childCount != 2) return false;
                if (baseChild.GetChild(0).GetComponent<TextMeshProUGUI>() == null) return false;
                return true;
            }
            catch (Exception e)
            {
                BLogger.WriteLineToLog(e.ToString());
            }

            return false;
        }
    }

    public static class SettingsColumnModdedHelper
    {
        public static ModdedSliderSetting SliderElement = null;
        public static ModdedToggleSetting ToggleElement = null;
    }

    public class ModdedSettingBase : MonoBehaviour
    {
        TextMeshProUGUI nameText;

        protected virtual void OnStart() { }

        bool enabled = false;

        void Awake() => Init();
        void Start() => Init();

        public void Init()
        {
            nameText = null;
            if (enabled) return;
            enabled = true;
            FindNametext();
            OnStart();
        }

        public void SetName(string name)
        {
            if (nameText == null) FindNametext();
            BLogger.WriteLineToLog(name);
            BLogger.WriteLineToLog(nameText == null ? "NAMETEXT NULL!" : "NAMETEXT NOTNULL!");
            try
            {
                gameObject.name = name;
                nameText.text = name;
            }catch(Exception e) { BLogger.WriteLineToLog($"Error: {e.Message}"); }
        }

        void FindNametext()
        {
            try
            {
                nameText = GetOn(transform.GetChild(0));
            }
            catch (Exception e) { BLogger.WriteLineToLog($"Error: {e.Message}"); }
        }

        protected TextMeshProUGUI GetOn(Transform getOn)
        {
            TextMeshProUGUI text = getOn.GetComponent<TextMeshProUGUI>();
            if (text == null) return null;
            text.SetText("{modname} SETTINGS!");

            LeanLocalizedTextMeshProUGUI lean = getOn.GetComponent<LeanLocalizedTextMeshProUGUI>();
            if (lean != null) Destroy(lean);

            return text;
        }
    }

    public class ModdedSliderSetting : ModdedSettingBase 
    {
        public Slider slider;
        public TextMeshProUGUI valueText;

        protected override void OnStart() 
        {
            slider = null;
            valueText = null;
            base.OnStart();
            try
            {
                slider = transform.GetChild(1).GetComponent<Slider>();
                valueText = GetOn(transform.GetChild(2));

                SettingSliderLockable settingSliderLockable = transform.GetChild(1).GetComponent<SettingSliderLockable>();
                if (settingSliderLockable != null) Destroy(settingSliderLockable);

                Settings_Slider settingsSlider = GetComponent<Settings_Slider>();
                if (settingsSlider != null) Destroy(settingsSlider);
            }
            catch (Exception e) { BLogger.WriteLineToLog($"Error: {e.Message}"); }
        }

        public void Cleanup(float min, float max, Action<float> callback, SliderType sliderType = SliderType.FLOAT)
        {
            if (slider == null) return;
            slider.onValueChanged.RemoveAllListeners();
            int persistentCount = slider.onValueChanged.GetPersistentEventCount();
            for (int i = 0; i < persistentCount; i++)
                slider.onValueChanged.SetPersistentListenerState(i, UnityEngine.Events.UnityEventCallState.Off);
            slider.minValue = min;
            slider.maxValue = max;

            slider.onValueChanged.AddListener((fValue) =>
            {
                callback?.Invoke(fValue);
                valueText?.SetText(fValue.ToString());
            });
        
            slider.wholeNumbers = sliderType == SliderType.INT;
            slider.onValueChanged?.Invoke(min);
        }
    }

    public class ModdedToggleSetting : ModdedSettingBase
    {
        public Toggle toggle;

        protected override void OnStart()
        {
            base.OnStart();
            try
            {
                toggle = transform.GetChild(1).GetComponent<Toggle>();

                Settings_Toggle sToggle = GetComponent<Settings_Toggle>();
                if (sToggle != null) Destroy(sToggle);
            }
            catch (Exception e) { BLogger.WriteLineToLog($"Error: {e.Message}"); }
        }
    }

    public enum SliderType
    {
        INT,
        FLOAT
    }
}
