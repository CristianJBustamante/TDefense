using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Pizia.StateMachine
{
    public abstract class StateAction : ScriptableObject
    {
        public abstract bool Execute();
    }
}