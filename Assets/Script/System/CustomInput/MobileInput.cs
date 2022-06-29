using System.Linq;
using UnityEngine;

namespace CustomInput
{
    public static class MobileInput
    {
        public static MobileInputAsset mobileInputAsset;

        private static bool hasAsset;

        public static float angle { get; set; }
        public static bool isDrag { get; set; }

        private static FloatAxis[] floatAxis;
        public static BoolAxis[] boolAxis;

        private static float absAngle => Mathf.Abs(angle);

        public static void Initialize(MobileInputAsset asset) 
        {
            mobileInputAsset = asset;

            hasAsset = mobileInputAsset != null;

            floatAxis = asset.GetFloatAxis();
            boolAxis = asset.GetBoolAxis();
        }

        public static float horizontal
        {
            get
            {
                if (!hasAsset) { return 0; }

                if (absAngle <= 40 && absAngle >= 0) { return 1f; }

                if (absAngle >= 130 && absAngle <= 180) { return -1f; }

                return 0;
            }
        }

        public static float vertical
        {
            get
            {
                if (!hasAsset) { return 0; }

                if (absAngle < 40 || absAngle > 140) { return 0; }

                if (angle > 0) { return 1; }

                if (angle < 0) { return -1; }

                return 0;
            }
        }

        #region Get Key

        public static bool GetKeyDown(KeyState keyState)
        {
            if (!hasAsset) { return false; }

            return boolAxis.Where(axis => axis.KeyState == keyState).First().KeyDown;
        }

        public static bool GetKey(KeyState keyState) 
        {
            if (!hasAsset) { return false; }

            return boolAxis.Where(axis => axis.KeyState == keyState).First().Key;
        }

        public static bool GetKeyUp(KeyState keyState)
        {
            if (!hasAsset) { return false; }

            return boolAxis.Where(axis => axis.KeyState == keyState).First().KeyUp;
        }

        #endregion

        #region Set Key

        public static void SetKeyDown(KeyState keyState, bool isPress)
        {
            if (!hasAsset) { return; }

            boolAxis.Where(axis => axis.KeyState == keyState).First().KeyDown = isPress;
        }

        public static void SetKey(KeyState keyState, bool isPress)
        {
            if (!hasAsset) { return; }

            boolAxis.Where(axis => axis.KeyState == keyState).First().Key = isPress;
        }

        public static void SetKeyUp(KeyState keyState, bool isPress)
        {
            if (!hasAsset) { return; }

            boolAxis.Where(axis => axis.KeyState == keyState).First().KeyUp = isPress;
        }

        #endregion
    }
}