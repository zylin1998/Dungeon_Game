using System.Linq;
using UnityEngine;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "Kill Goal Asset", menuName = "Quest/Quest Goal/Kill Goal",order = 1)]
    public class KillGoalAsset : QuestGoalAsset
    {
        #region Kill Packed

        [System.Serializable]
        public class KillPacked : Pack
        {
            protected KillPacked() {}

            public KillPacked (KillGoalAsset asset) 
            {
                this.targets = asset.targets;
            }
        }

        #endregion

        public override IPackableHandler.BasicPack Packed => new KillPacked(this);

        public override void Initialized()
        {
            targets
                .ToList()
                .ForEach(target => target
                .Initialize());
        }

        public override void Initialized(IPackableHandler.BasicPack basicPack)
        {
            if (basicPack is KillPacked pack)
            {
                pack.targets
                    .ToList()
                    .ForEach(target => this[target.TargetName]
                    .Initialize(target)); 
            }
        }

        public override bool IsReached()
        {
            return targets.Length == targets
                .Where(target => target.RequireAmount == target.CurrentAmount)
                .Count();
        }

        public override void AmountGathered(string targetName)
        {
            var target = this[targetName];

            if (target != null) { target.Gathered(); }
        }
    }
}