using System.Collections.Generic;
using UnityEngine;
using com.Pizia.Tools;

namespace com.Pizia.StateMachine
{
    public abstract class StateManager : CachedReferences
    {
        [SerializeField]
        protected StateData[] statesData;

        public State CurrentState { get; private set; }
        public State LastState { get; private set; }
        protected Dictionary<string, State> allStates = new Dictionary<string, State>();

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            for (int i = 0; i < statesData.Length; i++)
            {
                State temp = new State(statesData[i]);

                allStates.Add(statesData[i].name, temp);
            }

            SetState(statesData[0].name);
        }

        private void FixedUpdate()
        {
            FixedTick();
        }

        private void FixedTick()
        {
            if (CurrentState != null)
            {
                CurrentState.FixedTick();
            }
        }

        private void Update()
        {
            Tick();
        }

        private void Tick()
        {
            if (CurrentState != null)
            {
                CurrentState.Tick();
            }
        }

        private void LateUpdate()
        {
            ConditionsCheck();
        }

        private void ConditionsCheck()
        {            
            string targetState = CurrentState.CheckForStateChangeConditions();
            if (targetState != null)
                SetState(targetState);
        }

        State GetState(string stateId)
        {
            allStates.TryGetValue(stateId, out State value);
            return value;
        }

        public void SetState(string targetId)
        {
            if (CurrentState != null)
            {
                CurrentState.OnExit();
            }

            State targetState = GetState(targetId);

            if (targetState == null)
                Debug.LogError(targetId + " was not found!");

            LastState = CurrentState;
            CurrentState = targetState;

            if (CurrentState != null)
            {
                CurrentState.OnEnter();
            }
        }
    }
}
