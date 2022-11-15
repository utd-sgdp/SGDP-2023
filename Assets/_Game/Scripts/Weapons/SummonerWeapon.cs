using Game.Agent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Agent.Tree;
using Game.Enemy;

namespace Game.Weapons
{
    public class SummonerWeapon : WeaponBase
    {
        [Header("Stats")]
        [Header("References")]

        public GameObject summonedEnemy;

        protected override void OnAttack()
        {
            attackDuration = 5f;
            var summonerBlackboard = GetComponentInParent<AIAgent>().Tree.Blackboard;
            if (summonerBlackboard.transform.childCount < 5)
            {
                Vector3 summonerLocation = summonerBlackboard.transform.position;
                Vector3 spawnLocation = new Vector3(summonerLocation.x + UnityEngine.Random.Range(-3, 3), summonerLocation.y, summonerLocation.z + UnityEngine.Random.Range(-3, 3));
                Instantiate(summonedEnemy, spawnLocation, Quaternion.identity, summonerBlackboard.transform);
            }
            else 
            {
                attackDuration = 0f;
            }
        }

    }
}
