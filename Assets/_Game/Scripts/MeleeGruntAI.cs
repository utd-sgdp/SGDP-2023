using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MeleeGruntAI : MonoBehaviour
    {
        public float moveSpeed;
        Damageable PlayerHealth;
        Rigidbody rb;
        Transform target;

        public int attackDamageMin;
        public int attackDamageMax;

        public float attackCooldown;
        private float attackCooldownTimer;

        public float attackRange;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();

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

            //direction to move in
            Vector3 direction = (target.position - transform.position).normalized;

            //if the enemy is out of attack range, move towards the player
            if (distance > attackRange)
            {
                rb.velocity = direction * moveSpeed;
            }

            //if in attack range
            else
            {
                //stop moving
                rb.velocity = new Vector3(0, 0, 0);

                //Attack if the cooldown is over
                if (attackCooldownTimer > 0)
                {
                    attackCooldownTimer -= Time.deltaTime;
                }
                else
                {
                    attackCooldownTimer = attackCooldown;
                    attackTarget();
                }
            }
        }
        void attackTarget()
        {
            //hit the player for a random amount of damage
            PlayerHealth.Hurt(Random.Range(attackDamageMin, attackDamageMax));
        }
    }
}
