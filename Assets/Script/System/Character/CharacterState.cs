using System.IO;
using System.Collections.Generic;
using UnityEngine;
using ComponentPool;

namespace RoleSystem
{
    public class CharacterState : MonoBehaviour
    {
        [SerializeField] private string[] characterNames;
        [SerializeField] private int characterState;

        private GameObject parent;
        private List<GameObject> characters;
        private Transform currentCharacter;

        private TeleportSpots teleportSpots;
        private CameraFocus cameraController;

        public Transform CurrentCharacter => currentCharacter;

        private void Awake()
        {
            Components.Add(this, "CharacterState", EComponentGroup.Script);

            characters = new List<GameObject>();
        }

        private void Start()
        {
            teleportSpots = Components.GetStaff<TeleportSpots>("TeleportSpots", EComponentGroup.Script);
            cameraController = Components.GetStaff<CameraFocus>("CameraFocus", EComponentGroup.Script);

            Initialized();
        }

        private void Initialized()
        {
            parent = GameObject.Find("Player");

            if (characterNames.Length <= 0) { return; }

            foreach (string name in characterNames)
            {
                GameObject added = Resources.Load<GameObject>(Path.Combine("Character", name));

                if (added != null) { characters.Add(added); }
            }

            Invoke("SetCharacter", 0.05f);
        }

        private void SetCharacter()
        {
            Transform temp = currentCharacter == null ? null : currentCharacter;

            GameObject targetCharacter = null;

            var character = characters[characterState];
            var position = new Vector2(teleportSpots.CurrentSpot.position.x, teleportSpots.CurrentSpot.position.y + 0.425f);
            var rotation = Quaternion.identity;

            if (parent != null)
            {
                targetCharacter = Instantiate(character, position, rotation, parent.transform);

                targetCharacter.name = character.name;
            }

            if (targetCharacter != null)
            {
                currentCharacter = targetCharacter.transform;

                targetCharacter.tag = "Player";

                targetCharacter.GetComponent<RoleSystem.PlayerController>().IsFlip = teleportSpots.Flip;
            }

            if (currentCharacter != null) { cameraController.ChangeFocus(); }

            if (temp != null) { Destroy(temp); }
        }

        public void ChangeCharacter(int state)
        {
            if (state == characterState) { return; }

            characterState = state;

            SetCharacter();
        }
    }
}