using GameManagement;
using HarmonyLib;
using ReplayEditor;

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
                Main.UserInterfaceGameObject.SetActive(true);
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
                Main.UserInterfaceGameObject.SetActive(false);
            }
        }

        [HarmonyPatch(typeof(ReplayState), nameof(ReplayState.OnUpdate))]
        public static class OnUpdatePatch
        {
            /// <summary>
            /// Patching into OnUpdate to be able to track the camera position.
            /// </summary>
            static void Postfix()
            {
                var cameraController = ReplayEditorController.Instance?.cameraController;
                var camTransform = cameraController?.ReplayCamera?.transform;
                var time = ReplayEditorController.Instance?.playbackController?.CurrentTime;

                UserInterface.Instance.UpdateTextFields(camTransform, time);
                UserInterface.Instance.UpdateKeyFrameControls(cameraController?.keyFrames, time);
            }
        }
    }
}
