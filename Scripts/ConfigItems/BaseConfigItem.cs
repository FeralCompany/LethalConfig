using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BepInEx;
using BepInEx.Configuration;
using System.Linq;
using System;
using System.Reflection;
using LethalConfig.Mods;
using LethalConfig.ConfigItems.Options;

namespace LethalConfig.ConfigItems
{
    public abstract class BaseConfigItem
    {
        internal ConfigEntryBase BaseConfigEntry { get; private set; }
        internal bool RequiresRestart { get; private set; }
        internal Mod Owner { get; set; }

        internal string Section => BaseConfigEntry.Definition.Section;
        internal string Name => BaseConfigEntry.Definition.Key;
        internal string Description => BaseConfigEntry.Description.Description;

        internal object CurrentBoxedValue { get; set; }
        internal object OriginalBoxedValue => BaseConfigEntry.BoxedValue;
        internal object BoxedDefaultValue => BaseConfigEntry.DefaultValue;

        internal bool HasValueChanged => !CurrentBoxedValue.Equals(OriginalBoxedValue);

        internal abstract GameObject CreateGameObjectForConfig();

        internal void ApplyChanges()
        {
            BaseConfigEntry.BoxedValue = CurrentBoxedValue;
        }

        internal void CancelChanges()
        {
            CurrentBoxedValue = OriginalBoxedValue;
        }

        internal void ChangeToDefault()
        {
            CurrentBoxedValue = BoxedDefaultValue;
        }

        internal BaseConfigItem(ConfigEntryBase configEntry, BaseOptions options)
        {
            BaseConfigEntry = configEntry;
            RequiresRestart = options.RequiresRestart;
        }
    }

    public abstract class BaseValueConfigItem<T> : BaseConfigItem
    {
        internal ConfigEntry<T> ConfigEntry => (ConfigEntry<T>)BaseConfigEntry;
        internal T CurrentValue
        {
            get => (T)CurrentBoxedValue;
            set => CurrentBoxedValue = value;
        }
        internal T OriginalValue => ConfigEntry.Value;
        internal T Defaultvalue => (T)ConfigEntry.DefaultValue;

        internal BaseValueConfigItem(ConfigEntry<T> configEntry, BaseOptions options): base(configEntry, options)
        {
            CurrentValue = OriginalValue;
        }

        public override string ToString()
        {
            return $"{Owner.modInfo.Name}/{ConfigEntry.Definition.Section}/{ConfigEntry.Definition.Key}";
        }
    } 
}
