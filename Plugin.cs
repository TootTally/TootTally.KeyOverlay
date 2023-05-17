using BaboonAPI.Hooks.Initializer;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using TootTally.Graphics;
using TootTally.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TootTally.KeyOverlay
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("TootTally", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin, ITootTallyModule
    {
        public static Plugin Instance;

        private const string CONFIG_NAME = "KeyOverlay.cfg";
        private const string CONFIG_FIELD = "KeyOverlay";
        public Options option;
        public ConfigEntry<bool> ModuleConfigEnabled { get; set; }
        public bool IsConfigInitialized { get; set; }
        public string Name { get => PluginInfo.PLUGIN_NAME; set => Name = value; }

        public ManualLogSource GetLogger { get => Logger; }

        public void LogInfo(string msg) => Logger.LogInfo(msg);
        public void LogError(string msg) => Logger.LogError(msg);

        private void Awake()
        {
            if (Instance != null) return;
            Instance = this;

            GameInitializationEvent.Register(Info, TryInitialize);
        }

        private void TryInitialize()
        {
            // Bind to the TTModules Config for TootTally
            ModuleConfigEnabled = TootTally.Plugin.Instance.Config.Bind("Modules", "KeyOverlay", true, "Displays Key Pressed During A Song");
            // Attempt to add this module to the TTModules page in TrombSettings
            if (TootTally.Plugin.Instance.moduleSettings != null) OptionalTrombSettings.Add(TootTally.Plugin.Instance.moduleSettings, ModuleConfigEnabled);
            TootTally.Plugin.AddModule(this);
        }

        public void LoadModule()
        {
            Harmony.CreateAndPatchAll(typeof(KeyOverlay), PluginInfo.PLUGIN_GUID);
            LogInfo($"Module loaded!");
        }

        public void UnloadModule()
        {
            Harmony.UnpatchID(PluginInfo.PLUGIN_GUID);
            LogInfo($"Module unloaded!");
        }

        public static class KeyOverlay
        {
            private static List<KeyCode> _keyPressedList;
            private static CustomButton _keyPrefab;

            [HarmonyPatch(typeof(LevelSelectController), nameof(LevelSelectController.Start))]
            [HarmonyPostfix]
            public static void SetKeyOverlayPrefab(LevelSelectController __instance)
            {
                if (_keyPrefab != null) return;
                var tempObj = GameObjectFactory.CreateCustomButton(__instance.bgshape.transform, Vector2.zero, new Vector2(50, 50), "test", "tempObj"); //idk where to put the tempObj just put it somewhere random lmfao
                _keyPrefab = GameObject.Instantiate(tempObj);
                _keyPrefab.gameObject.name = "KeyOverlayPrefab";
                GameObject.DestroyImmediate(tempObj.gameObject);
                GameObject.DontDestroyOnLoad(_keyPrefab);
                //_keyPrefab.button.onClick = new Button.ButtonClickedEvent();
            }


            [HarmonyPatch(typeof(GameController), nameof(GameController.Start))]
            [HarmonyPostfix]
            public static void OnGameControllerStartSetupOverlay(GameController __instance)
            {
            }

        }

    }
}