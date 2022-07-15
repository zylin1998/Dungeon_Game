using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem 
{
    [System.Serializable]
    public class Dialogue 
    {
        [SerializeField]
        private string name;
        [SerializeField]
        private string sentence;

        public string Name => name;
        public string Sentence => sentence;
    }


    [CreateAssetMenu(fileName = "Dialogue Asset", menuName = "Dialogue/Dialogue Asset", order = 1)]
    public class DialogueAsset : ScriptableObject
    {
        [SerializeField]
        private Dialogue[] conversation;

        public Queue<Dialogue> Conversation => new Queue<Dialogue>(this.conversation);
    }
}