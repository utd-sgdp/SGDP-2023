using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BreakableScript : MonoBehaviour
    {
        [SerializeField, HighlightIfNull]
        protected GameObject unbroken_object;

        [SerializeField, HighlightIfNull]
        protected GameObject broken_object;

        [SerializeField]
        protected float breakingForce;
        [SerializeField]
        protected Vector3 ExplosionFactor;
        [SerializeField]
        protected float physicsDelay;
        [SerializeField]
        protected float despawnDelay;

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.impulse.magnitude > breakingForce)
            {
                Destroy(this.GetComponent<Rigidbody>());
                unbroken_object.SetActive(false);
                broken_object.SetActive(true);

                foreach(Transform child in broken_object.transform)
                {
                    child.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(Vector3.Scale(collision.impulse, ExplosionFactor), collision.GetContact(0).point);
                }

                if (physicsDelay > 0)
                {
                    Invoke(nameof(StopPhysics), physicsDelay);
                }

                if (despawnDelay > 0)
                {
                    Invoke(nameof(DespawnPieces), despawnDelay);
                }
            }
        }

        private void StopPhysics()
        {
            foreach (Transform child in broken_object.transform)
            {
                Destroy(child.gameObject.GetComponent<Rigidbody>());
            }
        }

        private void DespawnPieces()
        {
            Destroy(broken_object);
            Destroy(unbroken_object);
        }
    }
}
