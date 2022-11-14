using System.Collections;
using System.Collections.Generic;
using Game.Agent.Tree;

namespace Game.Agent.Action
{
    /// <summary>
    /// <see cref="BehaviourTree"/> access to <see cref="SightSensor.InSight(string)"/>.
    /// </summary>
    public class InLineOfSight : ActionNode
    {
        SightSensor _sensor
        {
            get
            {
                if (!b_sensor)
                {
                    b_sensor = GetComponent<SightSensor>();
                }

                return b_sensor;
            }
        }
        SightSensor b_sensor;

        protected override void OnStart() { }
        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            return _sensor.InSight(Blackboard.target.tag) ? State.Success : State.Failure;
        }
        
        public override IReadOnlyCollection<string> GetDependencies() => new[] { typeof(SightSensor).AssemblyQualifiedName };
    }
}