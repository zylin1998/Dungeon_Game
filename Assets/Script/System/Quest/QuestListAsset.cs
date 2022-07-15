using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
namespace QuestSystem 
{
    [CreateAssetMenu(fileName = "Quest List Asset", menuName = "Quest/Quest List Asset", order = 1)]
    public class QuestListAsset : ScriptableObject
    {
        #region Data Packed Class

        [System.Serializable]
        public class Packed
        {
            public QuestAsset.Packed[] quests;

            public Packed() { }

            public Packed(QuestAsset.Packed[] pack) 
            {
                this.quests = pack;
            }
        }

        #endregion

        [SerializeField]
        private QuestAsset[] quests;

        public int Count => this.quests.Length;

        public QuestAsset[] Copy => new List<QuestAsset>(this.quests).ToArray();

        public QuestAsset[] this[string targetScene] => this.quests.Where(quest => quest.TargetScene == targetScene).ToArray();

        public Packed Pack => new Packed(this.GetQuestNames());

        public void Initialize(QuestAsset[] quests) 
        {
            this.quests = quests;
        }

        public void Initialize(Packed pack) 
        {
            var quests = new List<QuestAsset>();

            foreach (QuestAsset.Packed quest in pack.quests)
            {
                var add = Resources.Load<QuestAsset>(Path.Combine("Quest", quest.quest));

                add.Initialize(quest);

                quests.Add(add);
            }

            this.quests = quests.ToArray();
        }

        private QuestAsset.Packed[] GetQuestNames() 
        {
            var questNames = new List<QuestAsset.Packed>();

            foreach (QuestAsset asset in quests) 
            {
                questNames.Add(asset.Pack);
            }

            return questNames.ToArray();
        }
    }
}