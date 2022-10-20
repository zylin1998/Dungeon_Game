using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInput;

public class InteractSpot : Spot
{
    protected Action InteractCallBack;

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (KeyManager.GetAxis("Vertical") != 0)
            {
                InteractCallBack?.Invoke();
            }
        }
    }
}
