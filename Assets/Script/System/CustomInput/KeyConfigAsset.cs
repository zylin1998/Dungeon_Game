using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace CustomInput
{
    #region Input Axes Class

    [System.Serializable]
    public class InputAxes 
    {
        [System.Serializable]
        public enum EAxesState 
        {
            Positive = 0,
            Negative = 1
        }

        [SerializeField]
        private string axesName;
        [SerializeField]
        private KeyCode positive;
        [SerializeField]
        private KeyCode negative;
        
        private float axes;

        public string AxesName => axesName;

        public float Axes 
        {
            get 
            {
                if (axes == 0 && Input.GetKey(positive)) { axes = 1f; }
                if (axes == 1 && !Input.GetKey(positive)) { axes = 0f; }

                if (axes == 0 && Input.GetKey(negative)) { axes = -1f; }
                if (axes == -1 && !Input.GetKey(negative)) { axes = 0f; }

                return axes;
            }
        }

        public bool GetKeyDown => Input.GetKeyDown(positive);
        public bool GetKey => Input.GetKey(positive);
        public bool GetKeyUp => Input.GetKeyUp(positive);

        public void SetAxes(EAxesState axesState, KeyCode keyCode) 
        {
            if (axesState == EAxesState.Positive) { positive = keyCode; }

            if (axesState == EAxesState.Negative) { negative = keyCode; }
        }
    }

    #endregion

    #region Input List Class

    [System.Serializable]
    public class InputList 
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
        private InputAxes[] axes;

        public EInputType InputType => inputType;

        public InputAxes this[string axesName] => axes.Where(a => a.AxesName == axesName).FirstOrDefault();

        public List<string> AxesNames => axes.ToList().ConvertAll(a => a.AxesName);

        public InputList Copy => new InputList(this);

        public InputList() { }

        public InputList(InputList list) 
        {
            this.inputType = list.inputType;
            this.axes = list.axes;
        }
    }

    #endregion

    #region Key Config Asset

    [CreateAssetMenu(fileName = "Key Input Asset", menuName = "System/Key Input Asset", order = 1)]
    public class KeyConfigAsset : PackableObject
    {
        #region Data Packed

        public class Pack : PackableObjectPack
        {
            public InputList[] inputList;

            protected Pack() { this.inputList = null; }

            public Pack(KeyConfigAsset asset) { this.inputList = asset.inputLists.ToArray(); }
        }

        #endregion

        [SerializeField]
        private List<InputList> inputLists;

        public bool isEmpty => inputLists.Count <= 0;

        public List<InputList> InputLists => inputLists;

        public InputList this[InputList.EInputType inputType] => inputLists.Find(input => input.InputType == inputType);

        public override IPackableHandler.BasicPack Packed => new Pack(this);

        public override void Initialized()
        {
            var list = Resources.Load<KeyConfigAsset>(Path.Combine("System", "CustomInput", "DefaultInput")).inputLists;

            this.inputLists = list.Select(l => l.Copy).ToList();

            KeyManager.Initialize(this);
        }

        public override void Initialized(IPackableHandler.BasicPack basicPack)
        {
            if (basicPack is Pack pack)
            {
                pack.UnPacked(this, packable => packable.inputLists = pack.inputList.ToList());
            }

            KeyManager.Initialize(this);
        }

        public List<InputAxes> GetAxes(string axes)
        {
            return inputLists.ConvertAll(list => list[axes]); 
        }
    }

    #endregion
}


