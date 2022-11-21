using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Agent
{
    public class SightSensor : MonoBehaviour
    {
        [SerializeField]
        Transform _eyes;

        [SerializeField]
        float _range;

        [SerializeField]
        float _sightRange;

        /// <summary>
        /// Wrapper of <see cref="InRange(Game.TransformData, Game.TransformData, float)"/>.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool InRange(TransformData target) => InRange(target, transform, _range);

        /// <summary>
        /// Checks if <see cref="target"/> is in sight of this <see cref="SightSensor"/>.
        /// </summary>
        /// <param name="target"> Unity Tag for target <see cref="GameObject"/> </param>
        /// <returns></returns>
        public bool InSight(string target)
        {
            // check if there are objects in front
            Ray ray = new(_eyes.position, _eyes.forward);
            if (!Physics.Raycast(ray, out var hitinfo, _sightRange))
            {
                return false;
            }

            // check if object is target
            Component t = hitinfo.rigidbody ? hitinfo.rigidbody : hitinfo.collider;
            return t.CompareTag(target);
        }

        static IEnumerable<Collider> GetCollidersInRange(Vector3 position, float range)
        {
            Collider[] colliders = new Collider[20];
            Physics.OverlapSphereNonAlloc(position, range, colliders);
            return colliders;
        }

        public IEnumerable<T> GetObjectsInRange<T>(Vector3 position, float range)
        {
            return from collider in GetCollidersInRange(position, range < 0 ? _sightRange : range)
                where collider != null
                select collider.GetComponent<T>()
                    into component
                    where component != null 
                    select component;
        }

        public IEnumerable<T> GetObjectsInSightRange<T>(Vector3 position) => GetObjectsInRange<T>(position, _sightRange);

        /// <summary>
        /// Simple distance check between <see cref="obj1"/> and <see cref="obj2"/>.
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static bool InRange(TransformData obj1, TransformData obj2, float range)
        {
            float distSQ = (obj1.Position - obj2.Position).sqrMagnitude;
            return distSQ < range * range;
        }
    }
}