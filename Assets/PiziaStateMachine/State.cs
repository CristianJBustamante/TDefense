using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace com.Pizia.StateMachine
{
    [System.Serializable]
    public class StateData
    {
        public string name;

        [Header("On Enter")]
        public StateAction[] onEnterActions;
        public UnityEvent onEnterEvents;

        [Header("On Fixed Update")]
        public StateAction[] fixedUpdateActions;
        public UnityEvent fixedEvents;

        [Header("On Update")]
        public StateAction[] updateActions;
        public UnityEvent updateEvents;

        [Header("On Exit")]
        public StateAction[] onExitActions;
        public UnityEvent onExitEvents;

        [Header("State Change Conditions")]
        public StateCondition[] stateChangeConditions;
    }

    public class State
    {
        public string Name { get; private set; }

        StateAction[] onEnterActions;
        UnityEvent onEnterEvents;

        StateAction[] fixedUpdateActions;
        UnityEvent fixedEvents;

        StateAction[] updateActions;
        UnityEvent updateEvents;

        StateAction[] onExitActions;
        UnityEvent onExitEvents;

        StateCondition[] stateChangeConditions;

        bool forceSkip;

        public State(StateData _data)
        {
            Name = _data.name;

            onEnterActions = _data.onEnterActions;
            onEnterEvents = _data.onEnterEvents;

            fixedUpdateActions = _data.fixedUpdateActions;
            fixedEvents = _data.fixedEvents;

            updateActions = _data.updateActions;
            updateEvents = _data.updateEvents;

            onExitActions = _data.onExitActions;
            onExitEvents = _data.onExitEvents;

            stateChangeConditions = _data.stateChangeConditions;
        }

        public void OnEnter()
        {
            ExecuteActionsArray(onEnterActions, onEnterEvents);
        }

        public void FixedTick()
        {
            ExecuteActionsArray(fixedUpdateActions, fixedEvents);
        }

        public void Tick()
        {
            ExecuteActionsArray(updateActions, updateEvents);
            forceSkip = false;
        }

        public void OnExit()
        {
            ExecuteActionsArray(onExitActions, onExitEvents);
        }

        void ExecuteActionsArray(StateAction[] _actionList, UnityEvent _events)
        {
            if (_actionList != null)
            {
                for (int i = 0; i < _actionList.Length; i++)
                {
                    if (forceSkip)
                        break;

                    forceSkip = _actionList[i].Execute();
                }
            }

            if (_events != null)
            {
                _events.Invoke();
            }
        }

        public string CheckForStateChangeConditions()
        {
            if (stateChangeConditions != null)
            {
                for (int i = 0; i < stateChangeConditions.Length; i++)
                {
                    if (stateChangeConditions[i].CheckForCondition().canChange)
                    {
                        return stateChangeConditions[i].CheckForCondition().targetState;
                    }
                }
                return null;
            }
            else
                return null;
        }
    }
}