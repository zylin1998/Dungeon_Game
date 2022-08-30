using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInform : MonoBehaviour
{
    #region Singleton

    public static PlayerInform Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) { return; }

        Instance = this;
    }

    #endregion

    [SerializeField]
    private Image LifeBar;
    [SerializeField]
    private Image MPBar;

    public void SetLife(float life) 
    {
        LifeBar.fillAmount = life;
    }

    public void SetMP(float mp)
    {
        MPBar.fillAmount = mp;
    }
}
