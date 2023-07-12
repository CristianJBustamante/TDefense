using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Pizia.StateMachine
{
    public class ConditionData
    {
        public bool canChange; 
        public string targetState;

        public ConditionData(bool _canChange, string _targetState)
        {
            canChange = _canChange;
            targetState = _targetState;
        }
    }

    public abstract class StateCondition : ScriptableObject
    {
        public string targetState;
        public abstract ConditionData CheckForCondition();
    }
}