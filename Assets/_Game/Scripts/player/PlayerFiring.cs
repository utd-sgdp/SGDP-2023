using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    [RequireComponent(typeof(PlayerInput), typeof(Rigidbody))]
    public class PlayerFiring : MonoBehaviour
    {
        //get input system and the prefab to be used for bullets (for now, before we have different, unique guns)
        PlayerInput _input;
        InputAction _fireAction;
        public GameObject bullet;
        Transform _gun;

        // Start is called before the first frame update
        private void Awake()
        {
            _input = GetComponent<PlayerInput>();
            _fireAction = _input.actions["Fire"];
            _gun = transform.Find("Gun");
        }
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            //check if the fire action was pressed
            if(_fireAction.WasPressedThisFrame() && _fireAction.IsPressed())
            {
                Fire();
            }
        }

        void Fire()
        {
            //instantiate bullet prefab at the position and rotation of the gun tip if it isn't null
            if (_gun == null) return;
            if (_gun.Find("gunTip") == null) return;
            Instantiate(bullet, _gun.Find("gunTip").position, _gun.Find("gunTip").rotation);
        }
    }
}
