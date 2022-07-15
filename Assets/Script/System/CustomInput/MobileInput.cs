using System.Linq;
using UnityEngine;

namespace CustomInput
{
    public static class MobileInput
    {
        public static MobileInputAsset mobileInputAsset;

        private static bool hasAsset;

        private static JoyStickAxes[] joySticks;
        private static ButtonAxis[] buttons;

        public static void Initialize(MobileInputAsset asset) 
        {
            mobileInputAsset = asset;

            hasAsset = mobileInputAsset != null;

            joySticks = asset.GetJoyStickAxes();
            buttons = asset.GetButtonAxes();
        }

        public static JoyStickAxes GetJoyStick(string axesName) 
        {
            return joySticks.Where(joystick => joystick.AxesName == axesName).FirstOrDefault();
        }

        #region Get Key

        public static bool GetButtonDown(string axesName)
        {
            if (!hasAsset) { return false; }

            return buttons.Where(axis => axis.AxesName == axesName).First().KeyDown;
        }

        public static bool GetButton(string axesName) 
        {
            if (!hasAsset) { return false; }

            return buttons.Where(axis => axis.AxesName == axesName).First().Key;
        }

        public static bool GetButtonUp(string axesName)
        {
            if (!hasAsset) { return false; }

            return buttons.Where(axis => axis.AxesName == axesName).First().KeyUp;
        }

        #endregion

        #region Set Key

        public static void SetButtonDown(string axesName, bool isPress)
        {
            if (!hasAsset) { return; }

            buttons.Where(axis => axis.AxesName == axesName).First().KeyDown = isPress;
        }

        public static void SetButton(string axesName, bool isPress)
        {
            if (!hasAsset) { return; }

            buttons.Where(axis => axis.AxesName == axesName).First().Key = isPress;
        }

        public static void SetButtonUp(string axesName, bool isPress)
        {
            if (!hasAsset) { return; }

            buttons.Where(axis => axis.AxesName == axesName).First().KeyUp = isPress;
        }

        #endregion

    }
}