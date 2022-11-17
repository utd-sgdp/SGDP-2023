using UnityEngine;

namespace Game.Weapons
{
    public class WeaponSummon : WeaponBase
    {
        [SerializeField]
        GameObject _summonPrefab;

        protected override void OnAttack()
        {
            Vector3 pos = transform.position;
            Vector3 summon = new Vector3(pos.x + Random.Range(-3, 3), pos.y, pos.z + Random.Range(-3, 3));
            Instantiate(_summonPrefab, summon, Quaternion.identity);
        }
        
        // TODO: add max summon condition
    }
}
