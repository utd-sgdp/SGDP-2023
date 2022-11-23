using System;
using Game.Play;
using Game.Play.Items.Statistics;
using UnityEngine;

namespace Game.Agent
{
    public class AgentStats : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        WeaponSensor DamageTarget;
        
        [SerializeField, ShowIf(nameof(DamageTarget))]
        Stat _damage = new();
        
        [SerializeField, HideInInspector]
        MovementSensor SpeedTarget;
        
        [SerializeField, ShowIf(nameof(SpeedTarget))]
        Stat _speed = new();
        
        #if UNITY_EDITOR
        void OnValidate()
        {
            DamageTarget = GetComponent<WeaponSensor>();
            SpeedTarget = GetComponent<MovementSensor>();
        }
        #endif
        
        // connect stat object to game behaviours
        void OnEnable()
        {
            if (DamageTarget != null) _damage.OnChange += DamageTarget.Value.OnStatChange;
            if (SpeedTarget != null) _speed.OnChange += SpeedTarget.OnStatChange;
        }

        void OnDisable()
        {
            if (DamageTarget != null) _damage.OnChange += DamageTarget.Value.OnStatChange;
            if (SpeedTarget != null) _speed.OnChange  -= SpeedTarget.OnStatChange;
        }
    }
}