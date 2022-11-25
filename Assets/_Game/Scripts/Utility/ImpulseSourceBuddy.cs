using Cinemachine;
using UnityEngine;

namespace Game.Utility
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class ImpulseSourceBuddy : MonoBehaviour
    {
        CinemachineImpulseSource _source;

        void Awake()
        {
            _source = GetComponent<CinemachineImpulseSource>();
        }

        public void GenerateWithRandomDirection() => GenerateWithRandomDirection(1);
        public void GenerateWithRandomDirection(float force)
        {
            Vector3 dir = new Vector3(
                Random.Range(0, 1f),
                Random.Range(0, 1f),
                // Random.Range(0, 1f)
                0
            );
            
            _source.GenerateImpulse(dir * force);
        }
    }
}