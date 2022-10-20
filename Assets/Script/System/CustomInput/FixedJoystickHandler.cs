using UnityEngine;
using UnityEngine.EventSystems;
using ComponentPool;


namespace CustomInput
{
    public class FixedJoystickHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        [SerializeField]
        private Transform content;
        [SerializeField]
        private string axesName;

        private JoyStickAxes axes;
        
        private void Awake()
        {
            CustomContainer.AddContent(this, "Input");
        }

        private void Start()
        {
            axes = MobileInput.GetJoyStick(axesName);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            axes.isDrag = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (this.content)
            {
                var direction = this.content.localPosition.normalized;

                axes.angle = Vector2.SignedAngle(Vector2.right, direction);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            axes.isDrag = false;
        }

        private void OnDestroy()
        {
            CustomContainer.RemoveContent(this, "Input");
        }
    }
}