using System;   
using UnityEngine;

namespace CustomInput
{
    public static class KeyManager
    {
        public static KeyConfigAsset keyConfigAsset;

        public static string horizontal = "Horizontal";
        public static string vertical = "Veritcal";

        private static KeyInput keyBoardInput;
        private static KeyInput joyStickInput;

        private static bool hasKeyBoard;
        private static bool hasJoyStick;

        private static Vector2 actionDirect = Vector2.zero;

        public static void Initialize(KeyConfigAsset asset) 
        {
            keyConfigAsset = asset;

            keyBoardInput = keyConfigAsset[KeyInput.EInputType.KeyBoard];
            joyStickInput = keyConfigAsset[KeyInput.EInputType.JoyStick];

            hasKeyBoard = keyBoardInput != null;
            hasJoyStick = joyStickInput != null;
        }

        #region Get Key OverLoad

        public static bool GetKey(string keyState) 
        {
            return GetKey((KeyState)Enum.Parse(typeof(KeyState), keyState));
        }

        public static bool GetKey(KeyState keyState) 
        {
            var keyBoard = hasKeyBoard ? keyBoardInput[keyState].KeyCode : KeyCode.None;
            var joyStick = hasJoyStick ? joyStickInput[keyState].KeyCode : KeyCode.None;

            var isKeyboardPress = keyBoard == KeyCode.None ? false : Input.GetKey(keyBoard);
            var isJoyStickPress = keyBoard == KeyCode.None ? false : Input.GetKey(joyStick);

            return isKeyboardPress || isJoyStickPress;
        }

        #endregion

        #region Get Key Down OverLoad

        public static bool GetKeyDown(string keyState)
        {
            return GetKeyDown((KeyState)Enum.Parse(typeof(KeyState), keyState));
        }

        public static bool GetKeyDown(KeyState keyState)
        {
            var keyBoard = hasKeyBoard ? keyBoardInput[keyState].KeyCode : KeyCode.None;
            var joyStick = hasJoyStick ? joyStickInput[keyState].KeyCode : KeyCode.None;

            var isKeyboardPress = keyBoard == KeyCode.None ? false : Input.GetKeyDown(keyBoard);
            var isJoyStickPress = keyBoard == KeyCode.None ? false : Input.GetKeyDown(joyStick);

            return isKeyboardPress || isJoyStickPress;
        }

        #endregion

        #region Get Key Up OverLoad

        public static bool GetKeyUp(string keyState)
        {
            return GetKeyUp((KeyState)Enum.Parse(typeof(KeyState), keyState));
        }

        public static bool GetKeyUp(KeyState keyState)
        {
            var keyBoard = hasKeyBoard ? keyBoardInput[keyState].KeyCode : KeyCode.None;
            var joyStick = hasJoyStick ? joyStickInput[keyState].KeyCode : KeyCode.None;

            var isKeyboardPress = keyBoard == KeyCode.None ? false : Input.GetKeyUp(keyBoard);
            var isJoyStickPress = keyBoard == KeyCode.None ? false : Input.GetKeyUp(joyStick);

            return isKeyboardPress || isJoyStickPress;
        }

        #endregion

        #region GetAxis

        public static float GetAxis(string axis) 
        {
            if (actionDirect.x == 0 && GetKey(KeyState.Right)) actionDirect.x = 1;
            if (actionDirect.x == 0 && GetKey(KeyState.Left)) actionDirect.x = -1;

            if (actionDirect.x == 1 && !GetKey(KeyState.Right)) actionDirect.x = 0;
            if (actionDirect.x == -1 && !GetKey(KeyState.Left)) actionDirect.x = 0;

            if (actionDirect.y == 0 && GetKey(KeyState.Up)) actionDirect.y = 1;
            if (actionDirect.y == 0 && GetKey(KeyState.Down)) actionDirect.y = -1;

            if (actionDirect.y == 1 && !GetKey(KeyState.Up)) actionDirect.y = 0;
            if (actionDirect.y == -1 && !GetKey(KeyState.Down)) actionDirect.y = 0;

            if (axis.ToLower() == horizontal.ToLower()) { return actionDirect.x; }
            if (axis.ToLower() == vertical.ToLower()) { return actionDirect.y; }

            return 0;
        }

        #endregion
    }
}