using UnityEngine;
using XLPrecisionKeyframes.Keyframes;

namespace XLPrecisionKeyframes.UserInterface.Popups
{
    public class UserInterfacePopup<T> where T : EditBaseUI
    {
        private readonly GameObject gameObject;
        private readonly T ui;

        public UserInterfacePopup()
        {
            gameObject = new GameObject();
            gameObject.SetActive(false);
            ui = gameObject.AddComponent<T>();
            Object.DontDestroyOnLoad(gameObject);
        }

        public void Destroy()
        {
            Object.DestroyImmediate(gameObject);
        }

        public void Save()
        {
            ui.Save();
        }

        #region SetValue
        public void SetValue(PositionInfo position)
        {
            ui.SetValue(position);
        }

        public void SetValue(RotationInfo rotation)
        {
            ui.SetValue(rotation);
        }

        public void SetValue(TimeInfo time)
        {
            ui.SetValue(time);
        }

        public void SetValue(FieldOfViewInfo fov)
        {
            ui.SetValue(fov);
        }
        #endregion

        #region Show()
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Show(PositionInfo position)
        {
            Show();
            SetValue(position);
        }

        public void Show(RotationInfo rotation)
        {
            Show();
            SetValue(rotation);
        }

        public void Show(TimeInfo time)
        {
            Show();
            SetValue(time);
        }

        public void Show(FieldOfViewInfo fov)
        {
            Show();
            SetValue(fov);
        }
        #endregion
    }
}
