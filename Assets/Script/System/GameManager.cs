using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ComponentPool;
using CustomInput;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) { return; }

        Instance = this;

        componentGroups = Components.Pool;

        Initialize();
    }

#endregion

    [SerializeField]
    private List<Components.StaffGroup> componentGroups;

    public QuestSystem.QuestPanel QuestPanel { get; private set; }

    public bool dialogueMode { get; set; }
    public bool inventoryMode { get; set; }
    public bool questMode { get; set; }

    public bool pause => dialogueMode || questMode || inventoryMode;
    
    #region Custom Input

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

    public void Initialize() 
    {
        mobileInput = Resources.Load<MobileInputAsset>(Path.Combine("System", "CustomInput", "MobileInput"));

        var mobileInputUI = Resources.Load<GameObject>(Path.Combine("System", "UI_Mobile_Input", "UI_Mobile_Input"));
        var parentCanvas = GameObject.Find("Canvas").transform;

        Instantiate(mobileInputUI, parentCanvas);

        MobileInput.Initialize(mobileInput);
    }

#endif

    #endregion
}
