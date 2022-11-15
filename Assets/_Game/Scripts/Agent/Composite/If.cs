using Game.Agent.Tree;
using UnityEngine;

namespace Game.Agent.Composite
{
    /// <summary>
    /// Perform operation (second child) while condition is met (first child)
    /// </summary>
    public class If : CompositeNode
    {
        [SerializeField]
        bool _completeActionBeforeReevaluating;
        
        bool _evaluated;
        State _result;

        protected override void OnStart() => _result = Children[0].Update(); 
        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            if (!_completeActionBeforeReevaluating)
            {
                _result = Children[0].Update();
            }
            
            // condition was met, perform operation
            if (_result == State.Success) return Children[1].Update();
            
            // propagate condition results
            return _result;
        }
    }
}
