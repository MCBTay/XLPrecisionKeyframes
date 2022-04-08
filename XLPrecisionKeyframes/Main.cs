using System.Linq;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace XLPrecisionKeyframes
{
#if DEBUG
    [EnableReloading]
#endif
    static class Main
    {
        public static bool Enabled;
        private static Harmony Harmony;

        public static bool XLGraphicsEnabled { get; private set; }

        public static GameObject UserInterfaceGameObject;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            Settings.Instance = UnityModManager.ModSettings.Load<Settings>(modEntry);
            Settings.ModEntry = modEntry;

            UserInterfaceGameObject = new GameObject();
            UserInterfaceGameObject.SetActive(false);
            UserInterfaceGameObject.AddComponent<UserInterface.UserInterface>();
            Object.DontDestroyOnLoad(UserInterfaceGameObject);

            modEntry.OnToggle = OnToggle;
            modEntry.OnGUI = Settings.Instance.OnSettingsGUI;
            modEntry.OnSaveGUI = Settings.Instance.Save;
#if DEBUG
            modEntry.OnUnload = Unload;
#endif

            return true;
        }

        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            if (Enabled == value) return true;
            Enabled = value;

            if (Enabled)
            {
                Harmony = new Harmony(modEntry.Info.Id);
                Harmony.PatchAll(Assembly.GetExecutingAssembly());

                var xlGraphics = UnityModManager.modEntries.FirstOrDefault(x => x.Info.Id == "XLGraphics");
                if (xlGraphics != null)
                {
                    XLGraphicsEnabled = xlGraphics.Enabled;
                }

                UnityModManager.Logger.Log($"XLPK: XLGraphicsEnabled = {XLGraphicsEnabled}.");
            }
            else
            {
                Object.DestroyImmediate(UserInterfaceGameObject);
                Harmony.UnpatchAll(Harmony.Id);
            }

            return true;
        }

#if DEBUG
        static bool Unload(UnityModManager.ModEntry modEntry)
        {
            Object.DestroyImmediate(UserInterfaceGameObject);

            Harmony?.UnpatchAll();
            return true;
        }
#endif
    }
}
