using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{

    public enum ObjectiveType
    {
        Kills,
        Time,
        Waves,
        Objective
    }

    [CreateAssetMenu(fileName = "New Goal", menuName = "Assets/Objectives/New Goal")]
    public class Goal : ScriptableObject
    {
        public ObjectiveType type;

        public float target; //number of kills needed, time limit, number of waves, number of objectives 
        
    }

    
    
}
