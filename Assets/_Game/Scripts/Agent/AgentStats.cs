using Game.Play;
using Game.Play.Items.Statistics;
using UnityEngine;

namespace Game.Agent
{
    public class AgentStats : MonoBehaviour
    {
        WeaponSensor DamageTarget;
        public Stat Damage = new();
        
        IStatTarget SpeedTarget;
        public Stat Speed = new();
        
        public Damageable Damageable { get; private set; }
        
        void Awake()
        {
            DamageTarget = GetComponent<WeaponSensor>();
            SpeedTarget = GetComponent<MovementSensor>();
            Damageable = GetComponentInChildren<Damageable>();
        }

        // connect stat object to game behaviours
        void OnEnable()
        {
            Damage.OnChange += DamageTarget.Value.OnStatChange;
            if (SpeedTarget != null) Speed.OnChange += SpeedTarget.OnStatChange;
        }

        void OnDisable()
        {
            Damage.OnChange += DamageTarget.Value.OnStatChange;
            if (SpeedTarget != null) Speed.OnChange  -= SpeedTarget.OnStatChange;
        }
    }
}