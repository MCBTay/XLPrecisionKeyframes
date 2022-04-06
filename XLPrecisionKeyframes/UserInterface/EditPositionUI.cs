using ReplayEditor;
using UnityEngine;
using XLPrecisionKeyframes.Keyframes;

namespace XLPrecisionKeyframes.UserInterface
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
            StartingYPos = 85;
            WindowLabel = "Edit Position";

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

            position.x = CreateFloatField("X", position.x);
            position.y = CreateFloatField("Y", position.y);
            position.z = CreateFloatField("Z", position.z);

            GUILayout.EndVertical();
        }
    }
}
