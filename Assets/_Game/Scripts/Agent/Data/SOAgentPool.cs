using UnityEngine;

namespace Game.Agent
{
    public class SOAgentPool : ScriptableObject
    {
        public SOAgent[] Agents => _agents;
        [SerializeField] SOAgent[] _agents;

        public static SOAgentPool Instantiate(SOAgent[] weapons)
        {
            SOAgentPool pool = CreateInstance<SOAgentPool>();
            LoadData(pool, weapons);

            return pool;
        }

        static void LoadData(SOAgentPool instance, SOAgent[] weapons)
        {
            instance._agents = weapons;
        }
    }
}