using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CustomInput;
using InventorySystem;
using QuestSystem;
using RoleSystem;

public interface IPageStateHandler 
{
    public bool PageState { get; }
}

public interface IPageStateDetected
{
    public List<IPageStateHandler> PageStates { get; }

    public bool IsPageOpen { get; }
}

public class GameManager : MonoBehaviour, IPageStateDetected
{
    #region Singleton

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) { return; }

        Instance = this;

        Initialize();

        PageStates = new List<IPageStateHandler>();

        if (initialData) { userData.Initialized(); }
    }

    #endregion

    [Header("玩家資料（存檔）")]
    [SerializeField]
    private UserData userData;
    [SerializeField]
    private bool initialData;
    [SerializeField]
    private UserOption userOption;

    public UserData UserData => userData;

    #region IPageStateDetected

    public List<IPageStateHandler> PageStates { get; private set; }

    public bool IsPageOpen => PageStates.Exists(page => page.PageState);

    #endregion

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

    public void AddPage(IPageStateHandler page) 
    {
        this.PageStates.Add(page);
    }

    public void ChangeScene(string scene) 
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void SaveUserData() 
    {
        var pack = this.userData.Packed as UserData.Pack;

        userOption = new UserOption(pack);

        var path = Path.Combine(Application.dataPath, "SaveData");
        var fileName = "UserData.json";

        userOption.Saved(path, fileName);
    }
}

[System.Serializable]
public class UserOption : SaveData 
{
    public ItemPool.Pack itemPool;
    public CharacterDetailAsset.Pack player;
    public QuestListAsset.Pack quest;
    public TeleportSpots.Pack teleportSpots;
    public ShopList.Pack shopList;

    protected UserOption() 
    {

    }

    public UserOption(ISaveHandler.SavePack savePack)
    {
        if (savePack is UserData.Pack pack)
        {
            this.itemPool = pack.GetData<ItemPool.Pack>();
            this.player = pack.GetData<CharacterDetailAsset.Pack>();
            this.quest = pack.GetData<QuestListAsset.Pack>();
            this.teleportSpots = pack.GetData<TeleportSpots.Pack>();
            this.shopList = pack.GetData<ShopList.Pack>();
        } 
    }

    public override ISaveHandler.SavePack GetPack()
    {
        var packs = new List<PackableObject.PackableObjectPack>();

        packs.Add(this.itemPool);
        packs.Add(this.player);
        packs.Add(this.quest);
        packs.Add(this.teleportSpots);
        packs.Add(this.shopList);

        return new UserData.Pack(packs);
    }
}

[System.Serializable]
public class SystemOption 
{

}