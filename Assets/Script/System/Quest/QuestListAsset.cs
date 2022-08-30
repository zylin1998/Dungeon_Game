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

            public Packed(QuestListAsset asset) 
            {
                this.quests = asset.quests.ToList().ConvertAll(quest => quest.Pack).ToArray();
            }
        }

        #endregion

        [SerializeField]
        private QuestAsset[] quests;

        public int Count => this.quests.Length;

        public QuestAsset[] Copy => new List<QuestAsset>(this.quests).ToArray();

        public QuestAsset[] this[string targetScene] => this.quests.Where(quest => quest.TargetScene == targetScene).ToArray();

        public Packed Pack => new Packed(this);

        public void Initialize(QuestAsset[] quests) 
        {
            this.quests = quests;

            this.quests.ToList().ForEach(quest => quest.Initialize());
        }

        public void Initialize(Packed pack) 
        {
            if (pack == null) { return; }

            var quests = new List<QuestAsset>();

            foreach (QuestAsset.Packed quest in pack.quests)
            {
                var add = Resources.Load<QuestAsset>(Path.Combine("Quest", quest.quest));

                add.Initialize(quest);

                quests.Add(add);
            }

            this.quests = quests.ToArray();
        }
    }
}