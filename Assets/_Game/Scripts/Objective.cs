using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "New Objective", menuName = "Assets/Objectives/New Objective")]
    public class Objective : ScriptableObject
    {
        public string title;
        public Goal[] goals;
    }
}
