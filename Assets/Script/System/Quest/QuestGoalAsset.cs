using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    [System.Serializable]
    public class QuestTarget 
    {
        [SerializeField]
        protected string targetName;
        [SerializeField]
        protected int requireAmount;
        [SerializeField]
        protected int currentAmount;

        public string TargetName => targetName;
        public int RequireAmount => requireAmount;
        public int CurrentAmount => currentAmount;

        public void Initialize() => this.currentAmount = 0;

        public void Initialize(QuestTarget target) => this.currentAmount = target.currentAmount;

        public void Gathered() => currentAmount++;

        public override string ToString() 
        {
            return string.Format("{0, -16}{1, 7}", targetName, $"({currentAmount}/{requireAmount})");
        }
    }

    public abstract class QuestGoalAsset : PackableObject
    {
        public abstract class Pack : PackableObjectPack
        {
            public QuestTarget[] targets;
        }

        [SerializeField]
        protected QuestTarget[] targets;

        public virtual QuestTarget this[string targetName] => this.targets.Where(target => target.TargetName == targetName).FirstOrDefault();

        public override IPackableHandler.BasicPack Packed => null;

        public abstract bool IsReached();

        public abstract void AmountGathered(string targetName);

        public override string ToString()
        {
            return string.Concat(targets.ToList().ConvertAll(t => t.ToString()));
        }
    }
}