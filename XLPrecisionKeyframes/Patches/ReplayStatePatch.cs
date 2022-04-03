using GameManagement;
using HarmonyLib;

namespace XLPrecisionKeyframes.Patches
{
    public static class ReplayStatePatch
    {
        [HarmonyPatch(typeof(ReplayState), nameof(ReplayState.OnEnter))]
        public static class OnEnterPatch
        {
            /// <summary>
            /// Patching into OnEnter to be able to create the user interface when entering ReplayState.
            /// </summary>
            static void Postfix()
            {
                UserInterface.Instance.Create();
            }
        }

        [HarmonyPatch(typeof(ReplayState), nameof(ReplayState.OnExit))]
        public static class OnExitPatch
        {
            /// <summary>
            /// Patching into OnExit to be able to destroy the user interface when exiting ReplayState.
            /// </summary>
            static void Postfix()
            {
                UserInterface.Instance.Destroy();
            }
        }
    }
}
