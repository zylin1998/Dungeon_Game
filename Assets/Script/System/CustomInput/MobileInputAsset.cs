using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace CustomInput
{
    #region Joysticks Axes

    [System.Serializable]
    public class JoyStickAxes 
    {
        [SerializeField]
        private string axesName;
        [SerializeField]
        private float actDegree;

        public float angle { get; set; }
        public bool isDrag { get; set; }

        private float absAngle => Mathf.Abs(angle);

        #region Getting Horizontal & Vertical

        public float Horizontal 
        {
            get 
            {
                if (!isDrag) { return 0; }

                if (absAngle >= 0f && absAngle <= actDegree) { return 1; }

                if (absAngle <= 180f && absAngle >= 180f - actDegree) { return -1; }

                return 0;
            }
        }

        public float Vertical 
        {
            get 
            {
                if (!isDrag) { return 0; }

                if (absAngle > 90f + actDegree) { return 0; }
                if (absAngle < 90f - actDegree) { return 0; }

                if (angle > 0) { return 1; }
                if (angle < 0) { return -1; }

                return 0;
            }
        }

        #endregion

        public string AxesName => axesName;
        public float ActDegree => actDegree;

        public JoyStickAxes Copy => new JoyStickAxes(this);

        public JoyStickAxes() { }

        public JoyStickAxes(JoyStickAxes axes) 
        {
            axesName = axes.AxesName;
            actDegree = axes.actDegree;
        }
    }

    #endregion

    #region Button Axis

    [System.Serializable]
    public class ButtonAxis 
    {
        [SerializeField]
        private string axesName;
        
        private bool keyUp;
        private bool key;
        private bool keyDown;
        

        public string AxesName => axesName;

        public bool KeyUp { get => keyUp; set => keyUp = value; }
        public bool Key { get => key; set => key = value; }
        public bool KeyDown { get => keyDown; set => keyDown = value; }

        public ButtonAxis Copy => new ButtonAxis(this);

        public ButtonAxis() { }

        public ButtonAxis(ButtonAxis axes)
        {
            this.axesName = axes.axesName;

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
        private JoyStickAxes[] joySticks;
        [SerializeField]
        private ButtonAxis[] buttons;

        public ButtonAxis GetButtontAxes(string axesName) => buttons.Where(axis => axis.AxesName == axesName).FirstOrDefault();

        public ButtonAxis[] GetButtonAxes() 
        {
            if (buttons.Length <= 0) { Debug.Log("BoolAxis is Null"); return null; }

            var axes = new List<ButtonAxis>();

            foreach(ButtonAxis button in buttons) 
            {
                axes.Add(button.Copy);
            }

            return axes.ToArray();
        }

        public JoyStickAxes GetJoyStickAxes(string axesName) => joySticks.Where(axis => axis.AxesName == axesName).FirstOrDefault();

        public JoyStickAxes[] GetJoyStickAxes()
        {
            if (joySticks.Length <= 0) { Debug.Log("BoolAxis is Null"); return null; }

            var axes = new List<JoyStickAxes>();

            foreach (JoyStickAxes joyStick in joySticks)
            {
                axes.Add(joyStick.Copy);
            }

            return axes.ToArray();
        }
    }
}