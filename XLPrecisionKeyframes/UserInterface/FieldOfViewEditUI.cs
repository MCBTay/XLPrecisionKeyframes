using UnityEngine;

namespace XLPrecisionKeyframes.UserInterface
{
    public class FieldOfViewEditUI : EditBaseUI
    {
        public float fov;
        public float originalFov;

        private string fovString;

        public void SetFov(float fov)
        {
            this.fov = fov;
            this.fovString = fov.ToString("F8");
            this.originalFov = fov;
        }

        protected override void OnGUI()
        {
            StartingYPos = 300;
            WindowLabel = "Edit FOV";

            base.OnGUI();
        }

        protected override void Save()
        {
            if (!float.TryParse(fovString, out var newFov)) return;

            //ReplayEditorController.Instance.SetPlaybackTime(newTime);

            base.Save();
        }

        protected override void CreateControls()
        {
            GUILayout.BeginVertical();

            fovString = CreateFloatField("Field of View", fovString);

            GUILayout.EndVertical();
        }
    }
}
