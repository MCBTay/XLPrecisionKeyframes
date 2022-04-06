using System;
using UnityEngine;
using UnityModManagerNet;

namespace XLPrecisionKeyframes
{
    [Serializable]
    public class Settings : UnityModManager.ModSettings
    {
        public static Settings Instance { get; set; }
        public static UnityModManager.ModEntry ModEntry;

        public float WindowXPos { get; set; } = 40;
        public float WindowYPos { get; set; } = 40;

        public Settings() : base()
        {

        }

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

        public void Save()
        {
            Save(ModEntry);
        }

        public void OnSettingsGUI(UnityModManager.ModEntry modEntry)
        {
            GUILayout.BeginVertical();

            GUILayout.Label("Window Position:");

            var rightAligned = new GUIStyle(GUI.skin.textField)
            {
                alignment = TextAnchor.MiddleRight
            };

            GUILayout.BeginHorizontal();
            GUILayout.Label("X:");
            if (float.TryParse(GUILayout.TextField(WindowXPos.ToString(), rightAligned), out float xPos))
            {
                WindowXPos = xPos;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Y:");
            if (float.TryParse(GUILayout.TextField(WindowYPos.ToString(), rightAligned), out float yPos))
            {
                WindowYPos = yPos;
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }
    }
}
