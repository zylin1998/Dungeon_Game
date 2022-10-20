using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IConfirmUIHandler<TContent>
{
    public Button Confirm { get; }
    public Button Cancel { get; }

    public IDetailPanelHandler<TContent> DetailPanel { get; }

    public TContent Content { get; }

    public Action ConfirmCallBack { get; set; }

    public void SetMessage(TContent content);

    public void ConfirmClick();

    public void CancelClick();

    public void UIState(bool state);
}

public interface IConfirmUICtrlHandler<TContent> 
{
    public IConfirmUIHandler<TContent> ConfirmHandler { get; }

    public void SendMessage(TContent message);

    public void SendMessage(TContent message, Action confirm);
}
