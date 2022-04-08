using ReplayEditor;
using UnityEngine;
using XLPrecisionKeyframes.Keyframes;

namespace XLPrecisionKeyframes.UserInterface.Popups
{
    public class EditPositionUI : EditBaseUI
    {
        public PositionInfo position { get; set; }
        public PositionInfo originalPosition { get; set; }

        public override void SetValue(PositionInfo position)
        {
            this.position = new PositionInfo(position);
            this.originalPosition = new PositionInfo(position);
        }

        protected override void OnGUI()
        {
            StartingYPos = 130;
            Label = WindowLabel.EditPosition;

            base.OnGUI();
        }

        public override void Save()
        {
            ReplayEditorController.Instance.cameraController.ReplayCamera.transform.position = position.ConvertToVector3();
            base.Save();
        }

        protected override void CreateControls()
        {
            GUILayout.BeginVertical();

            position.x = CreateFloatField(FieldLabel.X, position.x);
            position.y = CreateFloatField(FieldLabel.Y, position.y);
            position.z = CreateFloatField(FieldLabel.Z, position.z);

            GUILayout.EndVertical();
        }
    }
}
