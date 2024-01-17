using BowloutModManager.BowloutMod.Interfaces;
using System;

namespace BowloutModManager.BowloutMod
{
    public interface IBowloutMod : IDisposable
    {
        /// <summary>
        /// The name of your plugin.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// The version of your plugin.
        /// </summary>
        Version Version { get; }
        /// <summary>
        /// The description of your plugin.
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Your plugin settings
        /// </summary>
        IBowloutConfiguration Configuration { get; }
        // void Dispose() is handled by IDisposable
        /// <summary>
        /// Gets called when the mod is loaded.
        /// </summary>
        void OnSetup();
        /// <summary>
        /// Gets called when the user explicitely enables the plugin.
        /// </summary>
        void OnEnable();
        /// <summary>
        /// Gets called when the user explicitely disables the plugin.
        /// </summary>
        void OnDisable();
        /// <summary>
        /// Gets called every frame.
        /// </summary>
        void OnUpdate();
        /// <summary>
        /// Gets called every late frame.
        /// </summary>
        void OnLateUpdate();
        /// <summary>
        /// Gets called every Fixed Update
        /// </summary>
        void OnFixedUpdate();
    }
}
