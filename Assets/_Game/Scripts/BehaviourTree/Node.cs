using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.BehaviourTree
{
    public abstract class Node : ScriptableObject
    {
        public enum State
        {
            Running,
            Success,
            Failure
        }

        public State state = State.Running;
        public bool started = false;

        public State Update()
        {
            //Run initial behaviour
            if (!started)
            {
                OnStart();
                started = true;
            }

            state = OnUpdate();

            //Check if finished then return end state
            if (state == State.Failure || state == State.Success)
            {
                OnStop();
                started = false;
            }
            return state;

        }

        //Functions to be overriden for setup and end.
        protected abstract void OnStart();
        protected abstract void OnStop();

        protected abstract State OnUpdate();
    }
}
