﻿using ReplayEditor;
using UnityEngine;
using XLPrecisionKeyframes.Keyframes;

namespace XLPrecisionKeyframes.UserInterface
{
    public class EditPositionUI : EditBaseUI
    {
        public PositionInfo position;
        public PositionInfo originalPosition;

        public void SetPosition(PositionInfo position)
        {
            this.position = new PositionInfo(position);
            this.originalPosition = new PositionInfo(position);
        }

        protected override void OnGUI()
        {
            StartingYPos = 85;
            WindowLabel = "Edit Position";

            base.OnGUI();
        }

        protected override void DrawWindow(int windowID)
        {
            base.DrawWindow(windowID);

            GUILayout.BeginVertical();
            CreatePositionControls();
            CreateSaveAndCancelButtons();
            GUILayout.EndVertical();
        }

        protected override void Save()
        {
            ReplayEditorController.Instance.cameraController.VirtualCamera.transform.position = position.ConvertToVector3();
            base.Save();
        }

        private void CreatePositionControls()
        {
            GUILayout.BeginVertical();

            position.x = CreateFloatField("X", position.x);
            position.y = CreateFloatField("Y", position.y);
            position.z = CreateFloatField("Z", position.z);

            GUILayout.EndVertical();
        }
    }
}
