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
            Blackboard.movementReference = GameObject.FindGameObjectWithTag("Player").transform;
            //Run away from player if no enemies are healable or target has died
            if (Blackboard.target == null || Blackboard.target == GameObject.FindGameObjectWithTag("Player").transform)
            {
                var enemies = GameObject.FindGameObjectsWithTag("Enemy");

                foreach (GameObject enemy in enemies)
                {
                    Damageable test = enemy.GetComponent<Damageable>();
                    if (test._health < test._maxHealth && enemy != Blackboard.gameObject)
                    {
                        Blackboard.target = enemy.GetComponent<Transform>();
                        return State.Success;
                    }
                }
                Blackboard.target = GameObject.FindGameObjectWithTag("Player").transform;
                Blackboard.movementReference = null;
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
                    if (test._health < test._maxHealth && enemy != Blackboard.gameObject)
                    {
                        Blackboard.target = enemy.GetComponent<Transform>();
                        return State.Success;
                    }
                }
                return State.Success;
            }
            Blackboard.movementReference = null;
            return State.Failure;
        }

    }
}

