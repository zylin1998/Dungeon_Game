using System.Linq;
using UnityEngine;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "Kill Goal Asset", menuName = "Quest/Quest Goal/Kill Goal",order = 1)]
    public class KillGoalAsset : QuestGoalAsset
    {
        [System.Serializable]
        public class KillPacked : Packed 
        {
            protected KillPacked() {}

            public KillPacked (KillGoalAsset asset) 
            {
                this.targets = asset.targets;
            }
        }

        public override Packed Pack => new KillPacked(this);

        public override void Initialize(Packed pack) 
        {
            if (pack is KillPacked killPack)
            {
                foreach (QuestTarget target in killPack.targets) 
                {
                    this[target.TargetName].Initialize(target);
                }
            }
        }

        public override bool IsReached() 
        {
            return targets.Length == targets.Where(target => target.RequireAmount == target.CurrentAmount).ToArray().Length;
        }

        public override void AmountGathered(string targetName) 
        {
            var target = this[targetName];

            if (target != null) { target.Gathered(); }
        }
    }
}