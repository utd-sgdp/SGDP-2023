using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Agent
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MeleeGruntAI : MonoBehaviour
    {
        [Header("Tracking")]
        public Transform Target;

        [SerializeField]
        Transform _eyes;
        
        [SerializeField]
        [Min(0)]
        float _attackRange = 10;

        [Header("Debug")]
        [SerializeField, ReadOnly]
        bool _attacking;
        
        NavMeshAgent _agent;

        void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        void Start()
        {
            if (Target != null) return;

            Debug.LogError($"Enemy ({name}) is missing a target");
            this.enabled = false;
        }

        void Update()
        {
            // attack was performed
            // following is not necessary, so exit
            if (AttemptAttack()) return;
            
            FollowTarget();
        }

        bool AttemptAttack()
        {
            var t = transform;
            if (!InAttackRange(t.position, Target.position, _attackRange))
            {
                return false;
            }
            
            if (!InLineOfSight(_eyes.position, t.forward, _attackRange, Target.tag, out RaycastHit hitinfo))
            {
                return false;
            }
            
            // let weapon take control
            _agent.isStopped = true;
            _attacking = true;
            
            // weapon will need to need to update _attacking (with Attack property, method, callback, event whatever)
            // then _agent.isStopped = false
            // resume follow/attack loop

            return true;
        }
        
        void FollowTarget()
        {
            _agent.destination = Target.position;
        }

        static bool InAttackRange(Vector3 position, Vector3 target, float range)
        {
            float distanceSQ = (target - position).sqrMagnitude;
            return distanceSQ < range * range;
        }

        /// <summary>
        /// Checks for a line of sight between position and target.
        /// </summary>
        /// <param name="position"> world-space position </param>
        /// <param name="direction"> direction from position to target </param>
        /// <param name="range"> max-sight range </param>
        /// <param name="tag"> the target's tag </param>
        /// <param name="hitinfo"></param>
        /// <returns> success if line of sight </returns>
        static bool InLineOfSight(Vector3 position, Vector3 direction, float range, string tag, out RaycastHit hitinfo)
        {
            Ray ray = new(position, direction);
            if (Physics.Raycast(ray, out hitinfo, range))
            {
                Component target = hitinfo.rigidbody ? hitinfo.rigidbody : hitinfo.collider;
                return target.CompareTag(tag);
            }
            else
            {
                return false;
            }
        }
    }
}
