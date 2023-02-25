using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomInput
{
    [System.Serializable]
    public enum EGetKeyType
    {
        None,
        GetAxes,
        GetKeyUp,
        GetKey,
        GetKeyDown
    }

    public interface IInputClient 
    {
        [System.Serializable]
        public class RequireAxes 
        {
            [SerializeField]
            private string axesName;
            [SerializeField]
            private EGetKeyType getKeyType;
            
            public string AxesName => axesName;
            public EGetKeyType GetKeyType => getKeyType;
        }

        public List<RequireAxes> Axes { get; }

        public bool IsCurrent { get; set; }

        public void GetValue(IEnumerable<AxesValue<float>> values);

        public void GetValue(IEnumerable<AxesValue<bool>> values);
    }

    public struct AxesValue<TValue>
    {
        private object value;

        public string AxesName { get; private set; }
        public EGetKeyType GetKeyType { get; private set; }

        public TValue Value 
        { 
            get 
            {
                if (this.value == null) { return default(TValue); }

                if (value.GetType() == typeof(TValue)) { return (TValue)value; }

                return default(TValue);
            } 
        }

        public AxesValue(IInputClient.RequireAxes require) : this(require.AxesName, require.GetKeyType) 
        {
            var tvalue = typeof(TValue);

            if (tvalue == typeof(float)) { this.value = KeyManager.GetAxis(require.AxesName); }

            if (tvalue == typeof(bool)) { this.value = KeyManager.GetKey(require); }
        }

        public AxesValue(string axesName, EGetKeyType getKeyType) 
        {
            this.value = null;

            this.AxesName = axesName;
            this.GetKeyType = getKeyType;
        }

        public AxesValue(IInputClient.RequireAxes require, TValue value) : this(require.AxesName, require.GetKeyType, value) 
        {

        }

        public AxesValue(string axesName, EGetKeyType getKeyType, TValue value) : this(axesName, getKeyType)
        {
            this.value = value;
        }
    }

    //[RequireComponent(typeof(UICtrlClient))]
    public class InputClient : MonoBehaviour
    {
        public IInputClient Basic { get; private set; }
        public IInputClient Current { get; private set; }

        public Stack<IInputClient> Clients { get; private set; }

        public List<IInputClient.RequireAxes> Axes { get; private set; }
        public List<IInputClient.RequireAxes> Keys { get; private set; }

        public bool Pause => this.Current == null;

        private void Awake()
        {
            this.Clients = new Stack<IInputClient>();
        }

        private void Update()
        {
            if (this.Pause) { return; }

            var axes = this.Axes.ConvertAll(c => new AxesValue<float>(c));

            var keys = this.Keys.ConvertAll(c => new AxesValue<bool>(c));

            this.Current.GetValue(axes);

            this.Current.GetValue(keys);
        }

        #region Client Event

        public void PushClient(IInputClient client) 
        {
            this.Clients.Push(client);

            this.SetCurrent();
        }

        public void PopClient(IInputClient client) 
        {
            if (this.Clients.Peek().Equals(client)) { this.Clients.Pop().IsCurrent = false; }

            this.SetCurrent();
        }

        public void SetCurrent() 
        {
            if (this.Clients.Any()) 
            { 
                this.Current = this.Clients.Peek();

                this.Basic.IsCurrent = false;
            }

            else { this.Current = this.Basic; }

            this.Current.IsCurrent = true;

            this.Axes = this.Current.Axes.Where(a => a.GetKeyType == EGetKeyType.GetAxes).ToList();
            this.Keys = this.Current.Axes.Where(a => a.GetKeyType != EGetKeyType.None && a.GetKeyType != EGetKeyType.GetAxes).ToList();

            Debug.Log(this.Current.ToString());
        }

        public void SetBasic(IInputClient client) 
        {
            this.Basic = client;

            SetCurrent();
        }

        public void ClearBasic(IInputClient client) 
        {
            this.Basic = null;
        }

        public void ClearClient() 
        {
            this.Basic = null;
            this.Current = null;
            this.Clients.Clear();
        }

        #endregion

        public void OnDestroy()
        {
            KeyManager.StopUsedClient();
        }

        public override string ToString()
        {
            return string.Format("Basic: {0}\nCurrent: {1}\nStacks: {2}", this.Basic.ToString(), this.Current.ToString(), this.Clients.Count);
        }
    }
}