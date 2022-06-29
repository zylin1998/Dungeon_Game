using System.Linq;
using UnityEngine;

namespace CustomInput
{
    #region FloatAxis

    [System.Serializable]
    public class FloatAxis
    {
        [SerializeField]
        private KeyState keyState;
        [SerializeField]
        private float maxRange;
        [SerializeField]
        private float minRange;

        public KeyState KeyState => keyState;
        public float MaxRange => maxRange;
        public float MinRange => minRange;

        public FloatAxis() { }

        public FloatAxis(FloatAxis axis) 
        {
            this.keyState = axis.keyState;
            this.maxRange = axis.maxRange;
            this.minRange = axis.minRange;
        }

        public FloatAxis Copy() 
        {
            return new FloatAxis(this);
        }
    }

    #endregion

    #region Bool Axis

    [System.Serializable]
    public class BoolAxis 
    {
        [SerializeField]
        private KeyState keyState;
        
        private bool keyUp;
        private bool key;
        private bool keyDown;
        

        public KeyState KeyState => keyState;

        public bool KeyUp { get => keyUp; set => keyUp = value; }
        public bool Key { get => key; set => key = value; }
        public bool KeyDown { get => keyDown; set => keyDown = value; }

        public BoolAxis() { }

        public BoolAxis(KeyState keyState) 
        {
            this.keyState = keyState;

            this.keyUp = false;
            this.key = false;
            this.keyDown = false;
        }
    }

    #endregion

    [CreateAssetMenu(fileName = "Mobile Input Asset", menuName = "System/Mobile Input Asset", order = 1)]
    public class MobileInputAsset : ScriptableObject
    {
        [SerializeField]
        private FloatAxis[] floatAxis;
        [SerializeField]
        private KeyState[] boolAxis;

        public FloatAxis GetFloatAxis(KeyState keyState) => floatAxis.Where(axis => axis.KeyState == keyState).FirstOrDefault();

        public KeyState GetBooltAxis(KeyState keyState) => boolAxis.Where(axis => axis == keyState).FirstOrDefault();

        public BoolAxis[] GetBoolAxis() 
        {
            var length = boolAxis.Length;

            if (length <= 0) { Debug.Log("BoolAxis is Null"); return null; }

            var axis = new BoolAxis[length];

            for (int i = 0; i < length; i++) 
            {
                axis[i] = new BoolAxis(boolAxis[i]);
            }

            return axis;
        }

        public FloatAxis[] GetFloatAxis()
        {
            var length = floatAxis.Length;

            if (length <= 0) { return null; }

            var axis = new FloatAxis[length];

            for (int i = 0; i < length; i++)
            {
                axis[i] = floatAxis[i].Copy();
            }

            return axis;
        }
    }
}