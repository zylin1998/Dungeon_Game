using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoleSystem;

public class TargetSpot : Spot
{
    [SerializeField]
    private Transform host;

    private ITargetCheck target;
    
    public event Action TargetArmedCallBack = delegate { };

    protected override void Awake()
    {
        base.Awake();

        target = host.GetComponent<ITargetCheck>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            target.SetTarget(collision.transform);

            TargetArmedCallBack.Invoke();

            Destroy(this.gameObject);
        }
    }

}
