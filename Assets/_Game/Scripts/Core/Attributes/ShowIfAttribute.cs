using System;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Attribute will hide or show the field in the inspector depending on the Targets
    /// Targets can be any amount of strings, and will search for Boolean Fields of that name
    /// Usage:
    /// [SerializeField] private bool _useVolume;
    /// [SerializeField, ShowIf("_useVolume")] private float _volume;
    ///
    /// The second field, '_volume', will only appear in the inspector if '_useVolume' is true
    ///
    /// This can be stacked with other fields as well:
    /// [SerializeField, ShowIf("_check1", "check2")] private float _field;
    ///
    /// Credit: Brandon Coffey
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ShowIfAttribute : PropertyAttribute
    {
        public readonly string[] Targets;

        public ShowIfAttribute(params string[] t)
        {
            Targets = t;
        }
    }
}