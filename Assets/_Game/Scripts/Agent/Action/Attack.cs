using System.Collections.Generic;
using Game.Agent.Tree;
using Game.Weapons;

namespace Game.Agent.Action
{
    /// <summary>
    /// <see cref="BehaviourTree"/> access to <see cref="WeaponBase"/>. Stops the <see cref="UnityEngine.AI.NavMeshAgent"/> while
    /// attacking.
    /// </summary>
    public class Attack : ActionNode
    {
        WeaponSensor _weaponSensor;
        WeaponBase _weapon
        {
            get
            {
                if (!_weaponSensor)
                {
                    _weaponSensor = GetComponent<WeaponSensor>();
                }

                return _weaponSensor.Value;
            }
        }

        bool _isAttacking;
        
        // TODO: add field to idle till attack is ready

        protected override void OnStart()
        {
            _isAttacking = _weapon.AttemptAttack(AfterAttack: () =>
            {
                _isAttacking = false;
            });
        }

        protected override void OnStop() { }
        protected override State OnUpdate() => _isAttacking ? State.Running : State.Success;
        public override IReadOnlyCollection<string> GetDependencies() => new[] { typeof(WeaponSensor).AssemblyQualifiedName };
    }
}