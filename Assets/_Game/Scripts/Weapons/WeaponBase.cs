/*
    -Script to be inherited by weapons
    -Enemy and Player weapons
    -Will eventually manage weapon animations
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField]
    private float attackDuration;

    [SerializeField]
    private float cooldownDuration;

    //Attack takes arguments of <cooldown duration>
    public event System.Action<float> OnAttack;

    private bool attacking;
    private bool coolingDown;

    private bool AttemptAttack(System.Action<DelayType> callback = null)
    {
        if(attacking || coolingDown)
        {
            return false;
        }

        Attack(callback);
        return true;
    }

    private void Attack(System.Action<DelayType> callback)
    {
        StartCoroutine(AttackDelay(callback));
        OnAttack?.Invoke(cooldownDuration);
    }

    IEnumerator AttackDelay(System.Action<DelayType> callback)
    {
        if(callback == null)
        {
            yield break;
        }

        //Attack Animation time
        attacking = true;

        yield return new WaitForSeconds(attackDuration);
        callback.Invoke(DelayType.DuringAttack);

        //Attack Cooldown
        attacking = false;
        coolingDown = true;

        yield return new WaitForSeconds(cooldownDuration);
        callback.Invoke(DelayType.DuringCooldown);
        coolingDown = false;
    }

    public enum DelayType
    {
        DuringAttack, //Delay for attack animation/effect
        DuringCooldown //Delay for cooldown
    };
}

