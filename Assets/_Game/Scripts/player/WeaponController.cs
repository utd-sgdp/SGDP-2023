using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class WeaponController : MonoBehaviour
    {
        public GameObject Sword;

        [Header("Debug")]
        [SerializeField, ReadOnly]
        public bool CanAttack = true;

        public float AttackCooldown = 1.0f;

        PlayerInput _input;
        InputAction _attackAction;

        public bool IsAttacking = false;

        // Start is called before the first frame update
        void Awake()
        {
            _input = GetComponent<PlayerInput>();
            _attackAction = _input.actions["Fire"];
        }

        // Update is called once per frame
        void Update()
        {
            if(_attackAction.WasPressedThisFrame() && _attackAction.IsPressed() && CanAttack)
            {
                Attack();
            }
        }

        void Attack()
        {
            IsAttacking = true;
            CanAttack = false;
            Animator anim = Sword.GetComponent<Animator>();
            anim.SetTrigger("Attack");
            StartCoroutine(ResetAttackCooldown());
        }

        IEnumerator ResetAttackCooldown()
        {
            StartCoroutine(ResetAttackBool());
            yield return new WaitForSeconds(AttackCooldown);
            CanAttack = true;
        }

        IEnumerator ResetAttackBool()
        {
            yield return new WaitForSeconds(1.0f);
            IsAttacking = false;
        }
    }
}
