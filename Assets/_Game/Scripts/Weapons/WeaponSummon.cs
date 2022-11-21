using System.Collections.Generic;
using System.Linq;
using Game.Utility;
using UnityEngine;

namespace Game.Weapons
{
    public class WeaponSummon : WeaponBase
    {
        [SerializeField, Min(1)]
        int _maxSummons = 5;

        [SerializeField, Min(0)]
        float _summonRandomizer = 3;
        
        [SerializeField]
        Pool _summonPool;
        
        #if UNITY_EDITOR
        [SerializeField, ReadOnly]
        #endif
        List<GameObject> _summons = new();

        // TODO: return summons to pool when killed.
        protected override void OnAttack()
        {
            // prune dead enemies
            _summons = _summons.Where(summon => summon != null).ToList();
            
            // enforce max summon
            if (_summons.Count > _maxSummons) return;
            
            // spawn summon
            GameObject summon = _summonPool.CheckOut();
            _summons.Add(summon);
            
            Vector3 position = transform.position;
            position.x += Random.Range(-_summonRandomizer, _summonRandomizer);
            position.z += Random.Range(-_summonRandomizer, _summonRandomizer);
            
            summon.transform.SetPositionAndRotation(position, Quaternion.identity);
        }
    }
}
