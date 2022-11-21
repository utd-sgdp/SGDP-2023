// This has been taken aarthificals gist on GitHub:
// https://gist.github.com/aarthificial/f2dbb58e4dbafd0a93713a380b9612af
// It was also discussed in their YouTube video: https://www.youtube.com/watch?v=uZmWgQ7cLNI

using System;
using UnityEngine;

namespace Game.Utility
{
    /// <summary>
    /// A generic wrapper class for optional variables. This provides a consistent means
    /// of checking if a variable may be used. Expensive null checks are replaced with this.enabled.
    /// </summary>
    /// <remarks>
    /// Requires Unity 2020.1+ to serialize to the inspector.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Optional<T>
    {
        [SerializeField]
        internal bool _enabled;
        
        [SerializeField]
        internal T _value;

        public bool Enabled => _enabled;
        public T Value => _value;

        public Optional(bool enabled = false, T value = default)
        {
            _enabled = enabled;
            _value = value;
        }

        public Optional()
        {
            _enabled = false;
        }
    }
}