using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.Pizia.TouchJoypadSystem
{
    public class TouchJoypadButton : Selectable, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public delegate void EventHandler (TouchJoypadButton sender);
        public event EventHandler buttonTappedEvent;
        public event EventHandler buttonTouchedEvent;
        public event EventHandler buttonRapidFireEvent;
        public string buttonName;
        int? pressedByPointerId;
        public bool unpressedByMoveFinger = false;
        void ButtonTapped ()
        {
            buttonTappedEvent?.Invoke (this);
        }
        void ButtonTouched ()
        {
            buttonTouchedEvent?.Invoke (this);
        }
        void ButtonRapidFire ()
        {
            buttonRapidFireEvent?.Invoke (this);
        }
        void OnSelect ()
        {
            var eventSystem = EventSystem.current;
            if (eventSystem?.currentSelectedGameObject == gameObject)
            {
                eventSystem.SetSelectedGameObject (null);
            }
        }

        public override void OnPointerDown (PointerEventData eventData)
        {
            base.OnPointerDown (eventData);
            if (eventData.hovered.Contains (gameObject))
            {
                if (interactable)
                {
                    pressedByPointerId = eventData.pointerId;
                    ButtonTouched ();
                }
            }
        }

        public override void OnPointerUp (PointerEventData eventData)
        {
            base.OnPointerUp (eventData);
            if (eventData.pointerId == pressedByPointerId)
            {
                pressedByPointerId = null;
                if (eventData.hovered.Contains (gameObject))
                {
                    if (interactable)
                    {
                        ButtonTapped ();
                    }
                }
            }
        }

        public override void OnPointerExit (PointerEventData eventData)
        {
            base.OnPointerExit (eventData);
            if (eventData.pointerId == pressedByPointerId)
            {
                if (unpressedByMoveFinger)
                {
                    pressedByPointerId = null;
                    base.InstantClearState ();
                }
            }
        }
        void Update ()
        {
            if (pressedByPointerId != null)
            {
                if (interactable)
                {
                    ButtonRapidFire ();
                }
            }
        }
    }
}