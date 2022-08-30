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
    }

    public abstract class QuestGoalAsset : ScriptableObject
    {
        public abstract class Packed
        {
            public QuestTarget[] targets;
        }

        [SerializeField]
        protected QuestTarget[] targets;

        public virtual QuestTarget this[string targetName] => this.targets.Where(target => target.TargetName == targetName).FirstOrDefault();
        
        public virtual Packed Pack => null;

        public abstract void Initialize();

        public abstract void Initialize(Packed pack);

        public abstract bool IsReached();

        public abstract void AmountGathered(string targetName);
    }
}