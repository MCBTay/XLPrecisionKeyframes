using ReplayEditor;
using UnityEngine;
using XLPrecisionKeyframes.Keyframes;

namespace XLPrecisionKeyframes.UserInterface.Popups
{
    public class EditRotationUI : EditBaseUI
    {
        public RotationInfo originalRotation { get; set; }
        public RotationInfo rotation { get; set; }

        public override void SetValue(RotationInfo rotation)
        {
            this.rotation = new RotationInfo(rotation);
            this.originalRotation = new RotationInfo(rotation);
        }

        protected override void OnGUI()
        {
            StartingYPos = 195;
            Label = WindowLabel.EditRotation;

            base.OnGUI();
        }

        public override void Save()
        {
            ReplayEditorController.Instance.cameraController.ReplayCamera.transform.rotation = rotation.ConvertToQuaternion();
            base.Save();
        }

        protected override void CreateControls()
        {
            GUILayout.BeginVertical();

            rotation.x = CreateFloatField(FieldLabel.X, rotation.x);
            rotation.y = CreateFloatField(FieldLabel.Y, rotation.y);
            rotation.z = CreateFloatField(FieldLabel.Z, rotation.z);
            rotation.w = CreateFloatField(FieldLabel.W, rotation.w);

            GUILayout.EndVertical();
        }
    }
}
