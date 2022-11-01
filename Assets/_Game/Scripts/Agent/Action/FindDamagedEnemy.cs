using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Agent.Tree;
using System.Data;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Game.Agent.Action
{
    public class FindDamagedEnemy : ActionNode
    {
        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            if (Blackboard.target == null)
            {
                var enemies = GameObject.FindGameObjectsWithTag("Enemy");

                foreach (GameObject enemy in enemies)
                {
                    Damageable test = enemy.GetComponent<Damageable>();
                    if (test._health < test._maxHealth)
                    {
                        Blackboard.target = enemy.GetComponent<Transform>();
                        return State.Success;
                    }
                }
                //Prevent setting current to null object
                return State.Failure;
            }
            Damageable current = Blackboard.target.GetComponent<Damageable>();
            if (current._health < current._maxHealth)
            {
                return State.Success;
            }
            else if (current._health == current._maxHealth)
            {
                var enemies = GameObject.FindGameObjectsWithTag("Enemy");

                foreach (GameObject enemy in enemies)
                {
                    Damageable test = enemy.GetComponent<Damageable>();
                    if (test._health < test._maxHealth)
                    {
                        Blackboard.target = enemy.GetComponent<Transform>();
                        return State.Success;
                    }
                }
                return State.Success;
            }
            return State.Failure;
        }

    }
}

