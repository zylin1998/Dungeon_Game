using UnityEngine;
using InventorySystem;
using QuestSystem;
using RoleSystem;

[CreateAssetMenu(fileName = "User Data", menuName = "System/User Data", order = 1)]
public class UserData : ScriptableObject
{
    [System.Serializable]
    public class Packed 
    {
        public string scene;
        public string spot;

        public CharacterDetailAsset.Packed player;
        public QuestListAsset.Packed quest;
        public ItemPool.Packed items;

        protected Packed() { }

        public Packed(UserData asset) 
        {
            this.scene = asset.scene;
            this.spot = asset.spot;
            this.player = asset.player;
            this.quest = asset.quest;
            this.items = asset.items;
        }
    }

    [Header("各系統資料")]
    [SerializeField]
    private CharacterDetailAsset characterDetail;
    [SerializeField]
    private QuestListAsset questList;
    [SerializeField]
    private ItemPool itemPool;
    [Header("預設資料")]
    [SerializeField]
    private QuestListAsset defaultQuest;
    [Header("儲存資料")]
    [SerializeField]
    private string scene;
    [SerializeField]
    private string spot;
    [SerializeField]
    private CharacterDetailAsset.Packed player;
    [SerializeField]
    private QuestListAsset.Packed quest;
    [SerializeField]
    private ItemPool.Packed items;

    public string Scene => this.scene;
    public string Spot => this.spot;

    public Packed Pack => new Packed(this);

    public void Initialize() 
    {
        characterDetail.Initialize();
        questList.Initialize(defaultQuest.Copy);
        itemPool.Initialize();
    }

    public void Initialize(Packed packed) 
    {
        if (packed == null) { return; }

        this.scene = packed.scene;
        this.spot = packed.spot;
        this.player = packed.player;
        this.quest = packed.quest;
        this.items = packed.items;

        characterDetail.Initialize(this.player);
        questList.Initialize(this.quest);
        itemPool.Initialize(this.items);
    }

    public void SetData(string scene, string spot) 
    {
        this.scene = scene;
        this.spot = spot;
        this.player = characterDetail.Pack;
        this.quest = questList.Pack;
        this.items = itemPool.pack;
    }

    public void GoTo(string scene, string spot) 
    {
        this.scene = scene;
        this.spot = spot;
    }
}
