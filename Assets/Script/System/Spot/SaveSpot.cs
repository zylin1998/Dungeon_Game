using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInput;

public class SaveSpot : Spot
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        SpotManager.Instance.FirstEnter = true;
    }

    protected override void OnTriggerStay2D(Collider2D collision) 
    {
        if (collision.CompareTag("Player")) 
        {
            if (KeyManager.GetAxis("Vertical") != 0)
            {
                GameManager.Instance.SaveUserData();
            }
        }
    }
}
