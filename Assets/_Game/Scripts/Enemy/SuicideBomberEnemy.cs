using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy
{
    
    public class SuicideBomberEnemy : MonoBehaviour
    {
        /// <summary>
        /// Insert Variables.
        /// </summary>
        [SerializeField]
        NavMeshAgent _bomber;
        [SerializeField]
        GameObject _player;
        [SerializeField]
        int _range;

        /// <summary>
        /// 
        /// </summary>
        void Update()
        {
            float dist = Vector3.Distance(_player.transform.position, _bomber.transform.position);
            if(dist < _range)
            {
                _bomber.destination = _player.transform.position;
            }
        
        }
    }
}
