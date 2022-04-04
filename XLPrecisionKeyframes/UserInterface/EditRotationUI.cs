﻿using ReplayEditor;
using UnityEngine;
using XLPrecisionKeyframes.Keyframes;

namespace XLPrecisionKeyframes.UserInterface
{
    public class EditRotationUI : EditBaseUI
    {
        public RotationInfo originalRotation;
        public RotationInfo rotation;

        public void SetRotation(RotationInfo rotation)
        {
            this.rotation = new RotationInfo(rotation);
            this.originalRotation = new RotationInfo(rotation);
        }

        protected override void OnGUI()
        {
            StartingYPos = 170;
            WindowLabel = "Edit Rotation";

            base.OnGUI();
        }

        protected override void DrawWindow(int windowID)
        {
            base.DrawWindow(windowID);

            GUILayout.BeginVertical();
            CreateRotationControls();
            CreateSaveAndCancelButtons();
            GUILayout.EndVertical();
        }

        protected override void Save()
        {
            ReplayEditorController.Instance.cameraController.VirtualCamera.transform.rotation = rotation.ConvertToQuaternion();
            base.Save();
        }

        private void CreateRotationControls()
        {
            GUILayout.BeginVertical();

            rotation.x = CreateFloatField("X", rotation.x);
            rotation.y = CreateFloatField("Y", rotation.y);
            rotation.z = CreateFloatField("Z", rotation.z);
            rotation.w = CreateFloatField("W", rotation.w);

            GUILayout.EndVertical();
        }
    }
}
