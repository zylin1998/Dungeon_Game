using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace CustomInput
{
    public static class KeyManager
    {
        public static KeyConfigAsset keyConfigAsset { get; private set; }

        public static InputClient InputClient { get; private set; }

        public static bool HasInputClient => InputClient != null;

        private static bool hasKeyBoard;
        private static bool hasJoyStick;

        public static void Initialize(KeyConfigAsset asset) 
        {
            keyConfigAsset = asset;

            hasKeyBoard = keyConfigAsset[InputList.EInputType.KeyBoard] != null;
            hasJoyStick = keyConfigAsset[InputList.EInputType.JoyStick] != null;
        }

        #region Get Key

        public static bool GetKey(string axesName)
        {
            var axes = keyConfigAsset.GetAxes(axesName);

            if (axes != null) { return axes.Any(input => input.GetKey); }

            else { return false; }
        }

        public static bool GetKeyDown(string axesName)
        {
            var axes = keyConfigAsset.GetAxes(axesName);

            if (axes != null) { return axes.Any(input => input.GetKeyDown); }

            else { return false; }
        }

        public static bool GetKeyUp(string axesName)
        {
            var axes = keyConfigAsset.GetAxes(axesName);

            if (axes != null) { return axes.Any(input => input.GetKey); }

            else { return false; }
        }

        public static bool GetKey(IInputClient.RequireAxes require) 
        {
            if(require.GetKeyType == EGetKeyType.GetKey) { return GetKey(require.AxesName); }
            if(require.GetKeyType == EGetKeyType.GetKeyUp) { return GetKeyUp(require.AxesName); }
            if(require.GetKeyType == EGetKeyType.GetKeyDown) { return GetKeyDown(require.AxesName); }

            return false;
        }

        #endregion

        #region GetAxis

        public static float GetAxis(string axesName) 
        {
            var axes = keyConfigAsset.GetAxes(axesName);

            if (axes != null) { return axes.ConvertAll(input => input.Axes).Find(axes => axes != 0); }

            else { return 0; }
        }

        #endregion

        #region Input Client

        public static void InputClientCheck() 
        {
            if (InputClient == null) 
            {
                var inputClient = new GameObject("InputClient", new System.Type[] { typeof(InputClient) });

                InputClient = inputClient.GetComponent<InputClient>();

                Object.DontDestroyOnLoad(inputClient);
            }
        }

        public static void SetCurrent(IInputClient client, bool state)
        {
            InputClientCheck();

            if (state) { InputClient.PushClient(client); }

            if (!state) { InputClient.PopClient(client); }
        }

        public static void SetBasic(IInputClient client, bool state) 
        {
            InputClientCheck();

            if (state) { InputClient.SetBasic(client); }

            if (!state) { InputClient.ClearBasic(client); }
        }

        public static void ClearClient() 
        {
            if (HasInputClient) { InputClient.ClearClient(); }
        }

        public static void StopUsedClient() 
        {
            InputClient = null;
        }

        #endregion
    }
}