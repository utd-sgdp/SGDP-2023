using Game.Agent.Tree;
using Game.Play.Level;
using Game.Play;

namespace Game.Weapons
{
    public class WeaponHealer : WeaponBase
    {
        protected override void OnAttack()
        {
            // TODO: have projectile or sphere cast or something to find target
            gameObject.GetComponentInParent<AIAgent>().Tree.Blackboard.target.GetComponent<Damageable>().Heal(Damage);
        }
    }
}
