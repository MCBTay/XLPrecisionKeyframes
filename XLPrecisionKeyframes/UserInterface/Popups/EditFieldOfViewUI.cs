using ReplayEditor;
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
            StartingYPos = 455;
            Label = WindowLabel.EditFieldOfView;

            base.OnGUI();
        }

        public override void Save()
        {
            if (!float.TryParse(fovString, out var newFov)) return;

            ReplayEditorController.Instance.cameraController.ReplayCamera.fieldOfView = newFov;

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
