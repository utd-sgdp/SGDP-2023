using Game.Utility;
using UnityEngine;

namespace Game.Animation
{
    public class AnimatedSprite : MonoBehaviour
    {
        [SerializeField]
        SpriteAnimationData _animation;

        #if UNITY_EDITOR
        [SerializeField, ReadOnly]
        #endif
        int _currentFrame;
        
        [SerializeField]
        SpriteRenderer _renderer;

        float _timeSinceLastFrame;
        
        #if UNITY_EDITOR
        void OnValidate()
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
            if (_animation && _animation.Frames.Length > 0)
            {
                _renderer.sprite = _animation.Frames[0];
            }
        }
        #endif

        void Start()
        {
            _renderer.sprite = _animation.Frames[_currentFrame];
        }

        void Update()
        {
            if (_timeSinceLastFrame >= _animation.SPF)
            {
                NextFrame();
                return;
            }

            _timeSinceLastFrame += Time.deltaTime;
        }

        void NextFrame()
        {
            // increment frame
            _currentFrame++;
            _timeSinceLastFrame = 0;
            
            if (_currentFrame >= _animation.Frames.Length)
            {
                if (_pool)
                {
                    Despawn();
                    return;
                }

                _currentFrame = 0;
            }
            
            // show frame
            _renderer.sprite = _animation.Frames[_currentFrame];
        }

        Pool _pool;
        void OnSpawn(PoolAndTransform data)
        {
            Transform t = transform;
            t.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            t.SetParent(data.TransformData.Transform, worldPositionStays: false);
            t.SetWorldScale(data.TransformData.LocalScale);
            
            _timeSinceLastFrame = 0;
            _currentFrame = 0;
            _renderer.sprite = _animation.Frames[_currentFrame];
            
            _pool = data.Pool;
        }

        void Despawn()
        {
            if (!_pool) throw new System.NullReferenceException();
            _pool.CheckIn(gameObject, moveToScene: true);
            _pool = null;
        }
    }
}
