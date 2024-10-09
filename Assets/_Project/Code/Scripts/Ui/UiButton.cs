using DrawCar.Ui.Messages;
using MessagePipe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace DrawCar.Ui
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))] 
    public class UiButton : MonoBehaviour, IPointerUpHandler
    {
        [SerializeField] private EUiActionType _type;
        
        private OnUiActionMessage _message;
        private IPublisher<OnUiActionMessage> _publisher;
        
        [Inject]
        private void Construct(IPublisher<OnUiActionMessage> publisher)
        {
            _publisher = publisher;
        }
        
        internal virtual void Awake()
        {
            _message = new OnUiActionMessage(_type, true);
        }
        
        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
                _publisher.Publish(_message);
        }
    }
}