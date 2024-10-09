using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DrawCar.Ui
{
    [RequireComponent(typeof(Button))]
    public abstract class BaseButtonController : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerExitHandler
    { 
        public event Action OnPointerUpAction = () => { };
        public event Action OnPointerDownAction = () => { };
        public event Action OnPointerExitAction = () => { };
        
        internal bool IsLeftButton(in PointerEventData eventData)
        {
            return eventData.button == PointerEventData.InputButton.Left;
        }
        
        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if(IsLeftButton(eventData))
                OnPointerUpAction.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(IsLeftButton(eventData))
                OnPointerDownAction.Invoke();
        }
        
        public virtual void OnPointerExit(PointerEventData eventData)
        {
            OnPointerExitAction.Invoke();
        }
    }
}