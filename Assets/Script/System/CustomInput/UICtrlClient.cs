using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInput;

public class UICtrlClient : MonoBehaviour, IInputClient
{
    #region IInputClient

    [SerializeField]
    private List<IInputClient.RequireAxes> axes; 

    public List<IInputClient.RequireAxes> Axes => axes;

    public bool IsCurrent { get; set; }

    public void GetValue(IEnumerable<AxesValue<float>> values)
    {
        var direct = Vector2.zero;

        values.ToList().ForEach(v =>
        {
            if (v.AxesName == "Horizontal" && v.Value != 0) { direct.x = v.Value; }

            if (v.AxesName == "Vertical" && v.Value != 0) { direct.y = v.Value; }
        });
    }

    public void GetValue(IEnumerable<AxesValue<bool>> values)
    {
        values.ToList().ForEach(v =>
        {
            if (v.AxesName == "Attack" && v.Value) {  }

            if (v.AxesName == "Jump" && v.Value) {  }
        });
    }

    #endregion

    public Stack<IInputClient> UIClients { get; set; }
}
