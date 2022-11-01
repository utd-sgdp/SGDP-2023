using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Door : MonoBehaviour
    {
        MeshRenderer mesh = null;
        BoxCollider col = null;

        // Start is called before the first frame update
        void Start()
        {
            mesh = GetComponent<MeshRenderer>();
            col = GetComponent<BoxCollider>();
            Open();
        }

        public void Open()
        {
            mesh.enabled = false;
            col.enabled = false;
        }

        public void Close()
        {
            mesh.enabled = true;
            col.enabled = true;
        }
    }


}
