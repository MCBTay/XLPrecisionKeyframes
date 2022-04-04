﻿using ReplayEditor;
using UnityEngine;
using XLPrecisionKeyframes.Keyframes;

namespace XLPrecisionKeyframes.UserInterface
{
    public class EditFieldOfViewUI : EditBaseUI
    {
        public float fov;
        public float originalFov;

        private string fovString;

        public override void SetValue(FieldOfViewInfo fov)
        {
            this.fov = fov.fov;
            this.fovString = fov.fov.ToString("F5");
            this.originalFov = fov.fov;
        }

        protected override void OnGUI()
        {
            StartingYPos = 345;
            WindowLabel = "Edit FOV";

            base.OnGUI();
        }

        protected override void Save()
        {
            if (!float.TryParse(fovString, out var newFov)) return;

            ReplayEditorController.Instance.cameraController.ReplayCamera.fieldOfView = newFov;

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
