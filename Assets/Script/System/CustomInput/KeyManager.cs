namespace CustomInput
{
    public static class KeyManager
    {
        public static KeyConfigAsset keyConfigAsset;

        public static InputList keyBoardInput { get; private set; }
        public static InputList joyStickInput { get; private set; }

        private static bool hasKeyBoard;
        private static bool hasJoyStick;

        public static void Initialize(KeyConfigAsset asset) 
        {
            keyConfigAsset = asset;

            keyBoardInput = keyConfigAsset[InputList.EInputType.KeyBoard];
            joyStickInput = keyConfigAsset[InputList.EInputType.JoyStick];

            hasKeyBoard = keyBoardInput != null;
            hasJoyStick = joyStickInput != null;
        }

        public static bool GetKey(string axesName)
        {
            var keyBoard = hasKeyBoard ? keyBoardInput[axesName].GetKey : false;
            var joyStick = hasJoyStick ? joyStickInput[axesName].GetKey : false;

            return keyBoard || joyStick;
        }

        public static bool GetKeyDown(string axesName)
        {
            var keyBoard = hasKeyBoard ? keyBoardInput[axesName].GetKeyDown : false;
            var joyStick = hasJoyStick ? joyStickInput[axesName].GetKeyDown : false;

            return keyBoard || joyStick;
        }

        public static bool GetKeyUp(string axesName)
        {
            var keyBoard = hasKeyBoard ? keyBoardInput[axesName].GetKeyUp : false;
            var joyStick = hasJoyStick ? joyStickInput[axesName].GetKeyUp : false;

            return keyBoard || joyStick;
        }

        #region GetAxis

        public static float GetAxis(string axesName) 
        {
            var keyBoard = hasKeyBoard ? keyBoardInput[axesName].Axes : 0;
            var joyStick = hasJoyStick ? joyStickInput[axesName].Axes : 0;

            if (keyBoard != 0) { return keyBoard; }
            if (joyStick != 0) { return joyStick; }

            return 0;
        }

        #endregion
    }
}