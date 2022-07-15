using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CustomInput
{
    public class MobileInputButton : MonoBehaviour, IPointerDownHandler , IPointerUpHandler
    {
        [SerializeField]
        private string axesName;

        public void OnPointerDown(PointerEventData eventData)
        {
            MobileInput.SetButtonDown(axesName, true);

            StartCoroutine(Holding());
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            MobileInput.SetButton(axesName, false);

            StopCoroutine(Holding());

        }

        private IEnumerator Holding() 
        {
            yield return new WaitForEndOfFrame();

            MobileInput.SetButtonDown(axesName, false);
            MobileInput.SetButton(axesName, true);

            while (true) { yield return null; }
        }
    }
}