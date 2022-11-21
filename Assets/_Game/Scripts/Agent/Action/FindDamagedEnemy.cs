using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Agent.Tree;
using Game.Play.Level;
using Game.Play;

namespace Game.Agent.Action
{
    public class FindDamagedEnemy : ActionNode
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
        
        Damageable _self
        {
            get
            {
                if (!b_self)
                {
                    b_self = GetComponentInChildren<Damageable>();
                }

                return b_self;
            }
        }
        Damageable b_self;
        
        protected override void OnStart() { }
        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            List<Damageable> damageables = _sensor.GetObjectsInSightRange<Damageable>(transform.position).ToList();
            damageables.Remove(_self);
            
            Damageable[] allies = damageables.Where(damageable => damageable.gameObject.layer == gameObject.layer).ToArray();

            // follow ally
            if (allies.Length > 0)
            {
                // prioritize a damaged ally
                Damageable[] damagedAllies = allies.Where(damageable => !damageable.AtMaxHealth).ToArray();
                if (damagedAllies.Length > 0)
                {
                    // TODO: pick closest ally?
                    allies = damagedAllies;
                }
                
                Blackboard.target = allies[Random.Range(0, damagedAllies.Length)].transform;
                return State.Success;
            }
            
            // run from enemy
            Damageable[] enemies = damageables.Where(damageable => damageable.gameObject.layer != gameObject.layer).ToArray();

            // there are no enemies :/
            if (enemies.Length == 0)
            {
                Blackboard.target = null;
                return State.Failure;
            }
            
            Blackboard.target = enemies[Random.Range(0, enemies.Length)].transform;
            return State.Failure;
        }

        public override IReadOnlyCollection<string> GetDependencies() => new[] { typeof(SightSensor).AssemblyQualifiedName };
    }
}

