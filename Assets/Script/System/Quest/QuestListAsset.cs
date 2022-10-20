using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem 
{
    [CreateAssetMenu(fileName = "Quest List Asset", menuName = "Quest/Quest List Asset", order = 1)]
    public class QuestListAsset : PackableObject
    {
        [Serializable]
        public class Pack : PackableObjectPack
        {
            public QuestAsset.Pack[] quests;

            public Pack() { }

            public Pack(QuestListAsset asset) 
            {
                this.quests = asset.quests
                    .ConvertAll(quest => quest.Packed as QuestAsset.Pack)
                    .ToArray();
            }
        }

        [SerializeField]
        private ICategoryHandler.Category category;
        [SerializeField]
        private List<QuestAsset> quests;

        public int Count => this.quests.Count;

        public ICategoryHandler.Category Category => category;

        public override IPackableHandler.BasicPack Packed => new Pack(this);

        public Action<string> EnemyKillCallBack { get; set; }
        public Action<string> ItemPickUpCallBack { get; set; }

        public override void Initialized()
        {
            quests = Resources.Load<QuestListAsset>(Path.Combine("Quest", "Quest_List_Default")).GetList();

            quests.ForEach(quest => quest.Initialized());
        }

        public override void Initialized(IPackableHandler.BasicPack basicPack) 
        {
            QuestAsset LoadAsset(QuestAsset.Pack pack) 
            {
                var asset = Resources.Load<QuestAsset>(Path.Combine("Quest", pack.quest));

                if (pack != null) asset.Initialized(pack);

                else { asset.Initialized(); }

                return asset;
            }

            if (basicPack is Pack pack)
            {
                pack.UnPacked(this, packable => packable.quests = pack.quests.ToList().ConvertAll(LoadAsset));
            }
        }

        private List<QuestAsset> GetList() 
        {
            return new List<QuestAsset>(this.quests);
        }

        public List<QuestAsset> GetQuests(string scene, string giver) 
        {
            return this.quests
                .Where(s => s.Giver.scene == scene)
                .Where(q => q.Giver.giver == giver)
                .ToList();
        }

        public QuestAsset GetQuest(string scene, string giver) 
        {
            var quests = GetQuests(scene, giver);

            if (!quests.Any()) { return null; }

            var first = quests.FirstOrDefault();

            var progress = quests.Find(q => q.Detail.questState == "Progress");

            return progress == null ? first : progress;
        }

        public void RemoveQuest(QuestAsset quest) 
        {
            var asset = this.quests.Find(match => match.Equals(quest));

            if (asset.NewQuestAsset.Any()) { asset.NewQuestAsset.ForEach(add => this.quests.Add(add)); }

            quests.Remove(asset);
        }
    }
}