using System.Linq;
using UnityEngine;

namespace CustomInput 
{
    #region Key Types Enum

    public enum KeyState
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
        Attack = 4,
        Jump = 5,
        Dash = 6,
        Horizontal = 7,
        Vertical = 8,
        None
    }

    #endregion

    #region Key Frame

    [System.Serializable]
    public class KeyFrame
    {
        [SerializeField]
        private KeyState keyState;
        [SerializeField]
        private KeyCode keyCode;

        public KeyState KeyState => keyState;
        public KeyCode KeyCode => keyCode;
        
        public KeyFrame() => SetFrame(KeyState.None, KeyCode.None);

        public KeyFrame(KeyFrame frame) => SetFrame(frame.keyState, frame.keyCode);

        public KeyFrame(KeyState keyState, KeyCode keyCode) => SetFrame(keyState, keyCode);

        private void SetFrame(KeyState state, KeyCode code)
        {
            keyState = state;
            keyCode = code;
        }
    }

    #endregion

    #region Key Input Class

    [System.Serializable]
    public class KeyInput
    {
        [System.Serializable]
        public enum EInputType 
        {
            KeyBoard = 0,
            JoyStick = 1
        }

        [SerializeField]
        private EInputType inputType;
        [SerializeField]
        private KeyFrame[] keyFrames;

        #region Property

        public EInputType InputType => inputType;

        public KeyFrame this[KeyState state] => keyFrames.Where(frame => frame.KeyState == state).FirstOrDefault();

        public KeyFrame this[string state] => keyFrames.Where(frame => frame.KeyState.ToString().ToLower() == state.ToLower()).FirstOrDefault();

        #endregion

        public KeyInput() => keyFrames = null;

        public KeyInput(KeyInput config) => keyFrames = config.keyFrames;

        #region Public Function

        public KeyInput Copy()
        {
            KeyInput keyInput = new KeyInput();

            keyInput.inputType = inputType;
            keyInput.keyFrames = new KeyFrame[keyFrames.Length];

            for (int i = 0; i < keyFrames.Length; i++) { keyInput.keyFrames[i] = new KeyFrame(keyFrames[i]); }

            return keyInput;
        }

        public void Clear()
        {
            keyFrames = null;
        }

        public void Refresh(KeyInput input) 
        {
            keyFrames = input.keyFrames;
        }

        #endregion
    }

    #endregion

    #region Key Config Asset

    [CreateAssetMenu(fileName = "Key Input Asset", menuName = "System/Key Input Asset",order = 1)]
    public class KeyConfigAsset : ScriptableObject
    {
        #region Data Packed

        public class Pack 
        {
            public KeyInput[] keyInputs;

            protected Pack() { keyInputs = null; }

            public Pack(KeyConfigAsset asset) { keyInputs = asset.keyInputs; }
        }

        #endregion

        [SerializeField]
        private KeyInput[] keyInputs;

        public bool isEmpty => keyInputs.Length <= 0;

        public KeyInput this[int num] => keyInputs[num];

        public KeyInput this[KeyInput.EInputType inputType] => keyInputs.Where(input => input.InputType == inputType).FirstOrDefault();

        public Pack pack => new Pack(this);

        public void Initialize(KeyConfigAsset asset) 
        {
            var length = asset.keyInputs.Length;

            keyInputs = new KeyInput[length];

            for (int i = 0; i < length; i++) { keyInputs[i] = asset[i].Copy(); }
        }

        public void Initialize(Pack pack) 
        {
            keyInputs = pack.keyInputs;
        }
    }

    #endregion
}


