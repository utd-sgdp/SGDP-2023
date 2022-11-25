using System;
using UnityEngine;

namespace Game.Animation
{
    [CreateAssetMenu(menuName="Sprite Animation")]
    public class SpriteAnimationData : ScriptableObject
    {
        public Sprite[] Frames => _frames;
        public float FPS => _fps;
        public float SPF => _spf;

        [SerializeField]
        float _fps;
        
        [SerializeField]
        Sprite[] _frames;

        [SerializeField, HideInInspector]
        float _spf;
        
        #if UNITY_EDITOR
        void OnValidate()
        {
            _spf = 1f / _fps;
        }
        #endif
    }
}