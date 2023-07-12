using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.Pizia.Tools;

namespace com.Pizia.uiManager
{
    public class uiManager : CachedReferences
    {
        [SerializeField] private string startingState = "None";

        public static uiManager Instance;

        private uiState currentState;
        private Dictionary<string, uiState> states = new Dictionary<string, uiState>();

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                LoadUiStates();
            }
            else
            {
                Destroy(MyGameObject);
            }
        }

        private void Start()
        {
            ChangeState(startingState);
        }

        private void LoadUiStates()
        {
            foreach (Transform child in MyTransform)
            {
                uiState state = child.GetComponent<uiState>();
                if (state)
                {
                    if (startingState == "None") startingState = state.StateId;
                    states.Add(state.StateId, state);
                    state.SetState(false);
                }
            }
        }

        public void ChangeState(string _stateId)
        {
            ChangeState(_stateId, true, true);
        }

        public void ChangeState(string _stateId, bool _triggerEnterEvents, bool _triggerExitEvents)
        {
            if (states.Count <= 0)
            {
                Debug.LogError("There aren't any states loaded!");
                return;
            }

            uiState targetState = GetState(_stateId);

            if (!targetState)
            {
                Debug.LogError(_stateId + " was not found!");
                return;
            }

            float delay = 0f;
            if (currentState) delay = currentState.ExitState(_triggerExitEvents);

            currentState = targetState;

            StopAllCoroutines();
            StartCoroutine(_ChangeState(delay, _triggerEnterEvents));
        }

        IEnumerator _ChangeState(float delay, bool _triggerEnterEvents)
        {
            yield return new WaitForSecondsRealtime(delay);
            if (currentState) currentState.EnterState(_triggerEnterEvents);
        }

        private uiState GetState(string stateId)
        {
            states.TryGetValue(stateId, out uiState state);
            return state;
        }
    }
}