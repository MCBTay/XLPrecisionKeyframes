using ReplayEditor;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using XLPrecisionKeyframes.Keyframes;

namespace XLPrecisionKeyframes.UserInterface.Popups
{
    public class EditFieldOfViewUI : EditBaseUI
    {
        public float fov { get; set; }
        public float originalFov { get; set; }

        private string fovString;

        public override void SetValue(FieldOfViewInfo fov)
        {
            this.fov = fov.fov;
            this.fovString = fov.fov.ToString("F5");
            this.originalFov = fov.fov;
        }

        protected override void OnGUI()
        {
            StartingYPos = 435;
            Label = WindowLabel.EditFieldOfView;

            base.OnGUI();
        }

        public override void Save()
        {
            if (!float.TryParse(fovString, out var newFov)) return;

            if (Main.XLGraphicsEnabled)
            {
                var xlgraphics = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == "XLGraphics");
                var xlgraphicsui = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == "XLGraphicsUI");

                if (xlgraphics == null || xlgraphicsui == null) return;
                
                var customCamController = xlgraphics.GetType("XLGraphics.CustomEffects.CustomCameraController");
                var instance = customCamController?.GetProperty("Instance", customCamController)?.GetValue(null, null);
                var replay_fov = instance?.GetType().GetField("replay_fov");
                replay_fov?.SetValue(instance, newFov);

                var replayFovUI = xlgraphicsui.GetType("XLGraphicsUI.Elements.CameraUI.ReplayFovUI");
                var uiinstance = replayFovUI?.BaseType?.GetField("Instance")?.GetValue(null);
                var fov = replayFovUI?.GetField("fov").GetValue(uiinstance);
                var overrideValue = fov?.GetType().GetMethod("OverrideValue", BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof(float) }, null);
                overrideValue?.Invoke(fov, new object [] { newFov });
            }
            else
            {
                ReplayEditorController.Instance.cameraController.ReplayCamera.fieldOfView = newFov;
            }

            base.Save();
        }

        protected override void CreateControls()
        {
            GUILayout.BeginVertical();

            fovString = CreateFloatField(FieldLabel.FieldOfView, fovString);

            GUILayout.EndVertical();
        }
    }
}
