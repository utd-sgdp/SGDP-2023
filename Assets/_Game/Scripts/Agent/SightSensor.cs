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