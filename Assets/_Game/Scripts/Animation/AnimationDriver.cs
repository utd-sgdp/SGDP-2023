using UnityEngine;

namespace Game.Animation
{
    public class AnimationDriver : MonoBehaviour
    {
        [SerializeField]
        Animator _animator;

        #if UNITY_EDITOR
        [SerializeField, ReadOnly] bool _hasMovementProvider;
        [SerializeField, ReadOnly] bool _hasAttackProvider;
        #endif
        
        IMovementProvider _movementProvider;
        IWeaponProvider _weaponProvider;
        
        static readonly int FLOAT_VELOCITY_Z = Animator.StringToHash("Velocity Z");
        static readonly int FLOAT_VELOCITY_X = Animator.StringToHash("Velocity X");
        
        static readonly int TRIGGER_NONE = Animator.StringToHash("Attack None");
        static readonly int TRIGGER_ATTACK = Animator.StringToHash("Attack");
        static readonly int TRIGGER_RELOAD = Animator.StringToHash("Reload");

        #region MonoBehaviour
        #if UNITY_EDITOR
        void OnValidate()
        {
            _animator = GetComponentInChildren<Animator>();
            
            GetProviders();
            
            #if UNITY_EDITOR
            _hasMovementProvider = _movementProvider != null;
            _hasAttackProvider = _weaponProvider != null;
            #endif
        }
        #endif
        
        void OnEnable()
        {
            GetProviders();
            SubscribeToProviders();
        }

        void OnDisable()
        {
            UnsubscribeToProviders();
        }
        #endregion

        void GetProviders()
        {
            _movementProvider = GetComponent<IMovementProvider>();
            _weaponProvider = GetComponent<IWeaponProvider>();
        }

        void SubscribeToProviders()
        {
            _movementProvider?.SubscribeToOnMove(UpdateMovement);
            _weaponProvider?.Subscribe(UpdateWeapon);
        }
        
        void UnsubscribeToProviders()
        {
            _movementProvider?.UnsubscribeToOnMove(UpdateMovement);
            _weaponProvider?.Unsubscribe(UpdateWeapon);
        }

        void UpdateMovement(Vector2 move)
        {
            _animator.SetFloat(FLOAT_VELOCITY_X, move.x);
            _animator.SetFloat(FLOAT_VELOCITY_Z, move.y);
        }

        void UpdateWeapon(ActionType type)
        {
            switch (type)
            {
                default:
                case ActionType.None:
                    _animator.SetTrigger(TRIGGER_NONE);
                    break;
                
                case ActionType.Attack:
                    _animator.SetTrigger(TRIGGER_ATTACK);
                    break;
                
                case ActionType.Reload:
                    _animator.SetTrigger(TRIGGER_RELOAD);
                    break;
            }
        }
    }
}
