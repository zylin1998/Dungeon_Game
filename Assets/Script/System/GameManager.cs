using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ComponentPool;
using CustomInput;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager manager;

    private void Awake()
    {
        if (manager != null) { return; }

        manager = this;

        componentGroups = Components.Pool;

        Initialize();
    }

#endregion

    [SerializeField]
    private List<Components.StaffGroup> componentGroups;

#if UNITY_STANDALONE_WIN

    private KeyConfigAsset defaultInput;
    private KeyConfigAsset customInput;
    private KeyConfigAsset.Pack inputPack;

    private void Initialize()
    {
        defaultInput = Resources.Load<KeyConfigAsset>(Path.Combine("System", "CustomInput", "DefaultInput"));
        customInput = Resources.Load<KeyConfigAsset>(Path.Combine("System", "CustomInput", "CustomInput"));
        inputPack = SaveSystem.LoadBinaryData<KeyConfigAsset.Pack>(Path.Combine(Application.dataPath, "SaveData", "InputSeetting"));

        if (inputPack != null) { customInput.Initialize(inputPack); }

        if (customInput.isEmpty) { customInput.Initialize(defaultInput); }

        KeyManager.Initialize(customInput);
    }

#endif

#if UNITY_ANDROID

    private MobileInputAsset mobileInput;

    [SerializeField]
    private BoolAxis[] boolAxis;

    public void Initialize() 
    {
        mobileInput = Resources.Load<MobileInputAsset>(Path.Combine("System", "CustomInput", "MobileInput"));

        var mobileInputUI = Resources.Load<GameObject>(Path.Combine("System", "UI_Mobile_Input", "UI_Mobile_Input"));
        var parentCanvas = GameObject.Find("Canvas").transform;

        Instantiate(mobileInputUI, parentCanvas);

        MobileInput.Initialize(mobileInput);
    }

#endif
}
