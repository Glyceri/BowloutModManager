using Microsoft.Cci;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using static UnityEngine.Rendering.DebugUI;

namespace BowloutModManager.BowloutModded.CustomTypes
{
    [Serializable]
    public class BowloutBoolList
    {
        public bool[] values { get; private set; } = new bool[0];
        public string[] names { get; private set; } = new string[0];

        [JsonIgnore]
        public int Length => values.Length;

        public delegate void OnChangeMod(string name, bool value);
        public event OnChangeMod onChangeMod;

        public BowloutBoolList() { }

        [JsonConstructor]
        public BowloutBoolList(bool[] values, string[] names)
        {
            if (values == null || names == null)
            {
                this.values = new bool[0];
                this.names = new string[0];
                return;
            }

            if(names.Length != values.Length)
            {
                this.values = new bool[0];
                this.names = new string[0];
                return;
            }

            this.values = values;
            this.names = names;

            for(int i = 0; i < this.values.Length; i++)
            {
                onChangeMod?.Invoke(this.names[i], this.values[i]);
            }
        }

        [JsonIgnore]
        public BowloutBoolValue this[int i]
        {
            get => new BowloutBoolValue(values[i], names[i]);
            set
            {
                values[i] = value.value;
                names[i] = value.name;
                onChangeMod?.Invoke(names[i], values[i]);
            }
        }

        public void Remove(string name)
        {
            if (!this.names.Contains(name)) return;

            List<bool> values = this.values.ToList();
            List<string> names = this.names.ToList();
            int index = names.IndexOf(name);
            values.RemoveAt(index);
            names.RemoveAt(index);
            this.names = names.ToArray();
            this.values = values.ToArray();
        }
        
        public void Add(string name, bool defaultValue = true)
        {
            if (this.names.Contains(name)) return;

            List<bool> values = this.values.ToList();
            List<string> names = this.names.ToList();
            values.Add(defaultValue);
            names.Add(name);
            onChangeMod?.Invoke(name, defaultValue);
            this.values = values.ToArray();
            this.names = names.ToArray();
        }

        public void Set(string name, bool value)
        {
            for(int i = 0; i < names.Length; i++)
            {
                if (names[i] != name) continue;
                values[i] = value;
                onChangeMod?.Invoke(name, value);
                break;
            }
        }

        public void Set(List<BowloutBoolValue> values)
        {
            List<string> namesList = new List<string>();
            List<bool> valuesList = new List<bool>();
            foreach(BowloutBoolValue value in values)
            {
                namesList.Add(value.name);
                valuesList.Add(value.value);
                onChangeMod?.Invoke(value.name, value.value);
            }
            this.names = namesList.ToArray();
            this.values = valuesList.ToArray();
        }

        public void Clear()
        {
            values = new bool[0];
            names = new string[0];
        }

        public bool Contains(string name)
        {
            for(int i = 0; i < names.Length; i++)
            {
                if (names[i] == name) return true;
            }
            return false;
        }

        public bool Get(string name)
        {
            for (int i = 0; i < names.Length; i++)
            {
                if (names[i] == name) 
                    return values[i];
            }
            return true;
        }
    }

    public struct BowloutBoolValue 
    {
        public readonly bool value;
        public readonly string name;

        public BowloutBoolValue(bool value, string name)
        {
            this.value = value;
            this.name = name;
        }
    }
}
