using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using com.Pizia.Tools;


namespace com.Pizia.uiManager
{
    public class uiState : CachedReferences
    {
        [Header("State Settings")]
        [Space(5)]
        [SerializeField] private float enableDelay = 0.25f;
        [SerializeField] private float disableDelay = 0.25f;

        [Space(15)]
        [Header("State Events")]
        [Space(5)]
        [SerializeField] private UnityEvent onEnable;
        [SerializeField] private UnityEvent onDisable;

        private string stateId;
        public string StateId
        {
            get
            {
                if (stateId == null)
                {
                    stateId = MyGameObject.name;
                }
                return stateId;
            }
        }

        private CanvasGroup canvasGroup;
        private CanvasGroup CanvasGroup
        {
            get
            {
                if (!canvasGroup)
                {
                    canvasGroup = MyGameObject.AddComponent<CanvasGroup>();
                }
                return canvasGroup;
            }
        }

        private List<IuiElement> stateElements = new List<IuiElement>();

        private void Awake()
        {
            LoadUiElements();
        }

        public void SetState(bool _status)
        {
            if (_status)
            {
                CanvasGroup.alpha = 1;
                CanvasGroup.blocksRaycasts = true;
                CanvasGroup.interactable = true;
                MyGameObject.name = "(Shown) " + stateId;
            }
            else
            {
                CanvasGroup.alpha = 0;
                CanvasGroup.blocksRaycasts = false;
                CanvasGroup.interactable = false;
                MyGameObject.name = "(Hidden) " + stateId;
            }
        }

        private void LoadUiElements()
        {
            foreach (Transform child in MyTransform)
            {
                IuiElement element = child.GetComponent<IuiElement>();
                if (element != null)
                {
                    stateElements.Add(element);
                }
            }
        }

        public float EnterState(bool _triggerEvents)
        {
            StopAllCoroutines();
            StartCoroutine(_ShowElements(_triggerEvents));
            return enableDelay;
        }

        IEnumerator _ShowElements(bool _triggerEvents)
        {
            yield return new WaitForSecondsRealtime(enableDelay);
            SetState(true);
            if (_triggerEvents) onEnable?.Invoke();
            foreach (IuiElement element in stateElements)
            {
                element.SetElement();
                element.ShowElement();
            }
        }

        public float ExitState(bool _triggerEvents)
        {
            StopAllCoroutines();
            StartCoroutine(_HideElements(_triggerEvents));
            return disableDelay;
        }

        IEnumerator _HideElements(bool _triggerEvents)
        {
            foreach (IuiElement element in stateElements)
            {
                element.HideElement();
            }
            yield return new WaitForSecondsRealtime(disableDelay);
            SetState(false);
            if (_triggerEvents) onDisable?.Invoke();
        }
    }
}