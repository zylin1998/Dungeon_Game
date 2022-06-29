using UnityEngine;
using UnityEngine.EventSystems;
using ComponentPool;


namespace CustomInput
{
    public class FixedJoystickHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        [SerializeField]
        private Transform content;
        
        private void Awake()
        {
            Components.Add(this, "Mobile_JoyStick", EComponentGroup.Script);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            MobileInput.isDrag = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (this.content)
            {
                var direction = this.content.localPosition.normalized;

                MobileInput.angle = Vector2.SignedAngle(Vector2.right, direction);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            MobileInput.isDrag = false;
        }
    }
}