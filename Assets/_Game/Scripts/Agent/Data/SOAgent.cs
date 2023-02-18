using Game.Utility;
using UnityEngine;

namespace Game.Agent
{
    public enum AgentClass
    {
        Grunt = 0, Boss = 1,
    }
    
    public class SOAgent : ScriptableObject
    {
        public string Name => _data.Name;
        public AgentClass Class => _data.Class;
        public Optional<float> MaxHealth => _data.MaxHealth;
        public Optional<float> MoveSpeed => _data.MoveSpeed;
        public Optional<float> TurnSpeed => _data.TurnSpeed;
        public Optional<float> Acceleration => _data.Acceleration;
        public Optional<float> AttackRange => _data.AttackRange;
        public Optional<float> SightRange => _data.SightRange;

        [SerializeField] AgentData _data = new();
        
        public static SOAgent Instantiate(AgentData data)
        {
            SOAgent instance = CreateInstance<SOAgent>();
            
            instance._data = data;
            instance.name = data.Name;

            return instance;
        }
    }

    [System.Serializable]
    public class AgentData
    {
        public string Name;
        public AgentClass Class;
        
        public Optional<float> MaxHealth = new();
        public Optional<float> MoveSpeed = new();
        public Optional<float> TurnSpeed = new();
        public Optional<float> Acceleration = new();
        public Optional<float> AttackRange = new();
        public Optional<float> SightRange = new();
    }
}