using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CustomInput
{
    public class MobileInputButton : MonoBehaviour, IPointerDownHandler , IPointerUpHandler
    {
        [SerializeField]
        private KeyState keyState;

        public void OnPointerDown(PointerEventData eventData)
        {
            MobileInput.SetKeyDown(keyState, true);

            StartCoroutine(Holding());

        }

        public void OnPointerUp(PointerEventData eventData)
        {
            MobileInput.SetKey(keyState, false);

            StopCoroutine(Holding());

        }

        private IEnumerator Holding() 
        {
            yield return new WaitForEndOfFrame();

            MobileInput.SetKeyDown(keyState, false);
            MobileInput.SetKey(keyState, true);

            while (true) { yield return null; }
        }
    }
}