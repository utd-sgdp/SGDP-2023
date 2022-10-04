using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

namespace Game.Player
{
    //[RequireComponent(typeof(PlayerInput))]
    public class StarterWeapon : MonoBehaviour
    {
        //private Transform aimTransform;
        //Vector3 mousePosition;
        //Vector3 aimDirection;
        public Transform weaponPosition;
        public GameObject projectile;
        public float projectileForce;
        GameObject oldprojectile;
        //PlayerInput input;

        //public static Vector3 GetMouseWorldPosition()
        //{
        //Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //worldPosition.z = 0f;
        //return worldPosition;
        //}

        // Start is called before the first frame update
        void Awake()
        {
            Debug.Log("TEST");
            InvokeRepeating("OnFire", 2.0f, 1.0f);
            //input = GetComponent<PlayerInput>();
            //aimTransform = transform.Find("Aim");
        }

        // Update is called once per frame
        void Update()
        {
            //mousePosition = GetMouseWorldPosition();

            //aimDirection = (mousePosition - transform.position).normalized;
            //float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            //aimTransform.eulerAngles = new Vector3(0, 0, angle);
        }
        public void OnFire()
        {
            Destroy(oldprojectile);

            Debug.Log("FIRE!");
            GameObject firedProjectile = Instantiate(projectile, weaponPosition.position, weaponPosition.rotation);
            Rigidbody rb = firedProjectile.GetComponent<Rigidbody>();
            rb.AddForce(weaponPosition.up * projectileForce);

            oldprojectile = firedProjectile;
        }
    }
}