using System.Linq;
using UnityEngine;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "Kill Goal Asset", menuName = "Quest/Quest Goal/Kill Goal",order = 1)]
    public class KillGoalAsset : QuestGoalAsset
    {
        #region Kill Packed

        [System.Serializable]
        public class KillPacked : Packed 
        {
            protected KillPacked() {}

            public KillPacked (KillGoalAsset asset) 
            {
                this.targets = asset.targets;
            }
        }

        #endregion

        public override Packed Pack => new KillPacked(this);

        public override void Initialize() => targets.ToList().ForEach(target => target.Initialize());

        public override void Initialize(Packed pack) => (pack as KillPacked)?.targets.ToList().ForEach(target => this[target.TargetName].Initialize(target));

        public override bool IsReached() => targets.Length == targets.Where(target => target.RequireAmount == target.CurrentAmount).ToArray().Length;

        public override void AmountGathered(string targetName) => this[targetName]?.Gathered();
    }
}