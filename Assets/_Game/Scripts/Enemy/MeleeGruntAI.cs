using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy
{
    public class MeleeGruntAI : MonoBehaviour
    {
        Damageable PlayerHealth;
        private NavMeshAgent navMeshAgent;
        Transform target;

        public int attackDamageMin;
        public int attackDamageMax;

        public float attackCooldown;
        private float attackCooldownTimer;

        public float attackRange;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
          
            //prevents the enemy from attacking immediately when in range
            attackCooldownTimer = attackCooldown;
        }

        void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            PlayerHealth = target.GetComponent<Damageable>();
        }

        // Update is called once per frame
        void Update()
        {
            if (target)
            {
                followTarget();
            }
        }
        private void FixedUpdate()
        {

        }

        void followTarget()
        {
            //finds the player and looks at them
            Vector3 targetPosition = target.transform.position;
            targetPosition.y = transform.position.y;
            transform.LookAt(targetPosition);

            //calculates the distance between the enemy and the player
            float distance = Vector3.Distance(target.transform.position, this.transform.position);

            Ray vision = new Ray(transform.GetChild(0).transform.position, transform.forward);
            Debug.DrawRay(transform.GetChild(0).transform.position, transform.forward * attackRange);
            RaycastHit hit;


            //if the enemy is out of attack range, move towards the player
            if (distance > attackRange)
            {
                navMeshAgent.destination = targetPosition;
            }

            //if in attack range
            else if(Physics.Raycast(vision, out hit, attackRange))
            {
                if (hit.collider.tag == "Player")
                {
                    //stop moving
                    navMeshAgent.destination = transform.position;

                    //Attack if the cooldown is over
                    if (attackCooldownTimer > 0)
                    {
                        attackCooldownTimer -= Time.deltaTime;
                    }
                    else
                    {
                        attackCooldownTimer = attackCooldown;
                        //attack();
                    }
                }
            }
        }
    }
}
