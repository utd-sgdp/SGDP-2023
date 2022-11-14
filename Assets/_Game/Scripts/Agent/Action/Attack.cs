using System.Collections;
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

        protected override void OnStart()
        {
            _isAttacking = true;
            bool startedAttack = _weapon.AttemptAttack(AfterAttack: () =>
            {
                _isAttacking = false;
            });

            if (startedAttack)
            {
                _isAttacking = true;
            }
        }

        protected override void OnStop() { }
        protected override State OnUpdate() => _isAttacking ? State.Running : State.Success;
        public override IReadOnlyCollection<string> GetDependencies() => new[] { typeof(WeaponSensor).AssemblyQualifiedName };
    }
}