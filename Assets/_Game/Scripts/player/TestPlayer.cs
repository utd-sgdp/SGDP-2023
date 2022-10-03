using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TestPlayer : BasePlayer
    {
        private void Awake()
        {
            Debug.Log(GetHealth());
        }

        public override void Die()
        {
            base.Die();

            //on death
        }
    }
}
