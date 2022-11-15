using Game.Agent.Tree;
using UnityEngine;

namespace Game.Agent.Action
{
    public class Wait : ActionNode
    {
        [Min(0)]
        public float _duration = 1f;
        
        [SerializeField, ReadOnly]
        float _elapsedTime;

        protected override void OnStart() => _elapsedTime = 0;
        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            float elapsed = _elapsedTime;
            _elapsedTime += Time.deltaTime;
            return elapsed >= _duration ? State.Success : State.Running;
        }
    }
}
