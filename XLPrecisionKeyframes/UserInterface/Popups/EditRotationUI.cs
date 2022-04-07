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
            StartingYPos = 170;
            WindowLabel = "Edit Rotation";

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

            rotation.x = CreateFloatField("X", rotation.x);
            rotation.y = CreateFloatField("Y", rotation.y);
            rotation.z = CreateFloatField("Z", rotation.z);
            rotation.w = CreateFloatField("W", rotation.w);

            GUILayout.EndVertical();
        }
    }
}
