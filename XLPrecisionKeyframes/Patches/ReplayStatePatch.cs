using GameManagement;
using HarmonyLib;

namespace XLPrecisionKeyframes.Patches
{
    public static class ReplayStatePatch
    {
        [HarmonyPatch(typeof(ReplayState), nameof(ReplayState.OnEnter))]
        public static class OnEnterPatch
        {
            static void Postfix()
            {
                //TODO: create keyframe menu
            }
        }

        [HarmonyPatch(typeof(ReplayState), nameof(ReplayState.OnExit))]
        public static class OnExitPatch
        {
            static void Postfix()
            {
                //TODO: destroy keyframe menu
            }
        }
    }
}
